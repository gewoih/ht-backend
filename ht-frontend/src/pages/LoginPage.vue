<template>
  <div class="login-container">
    <div class="auth-card">
      <h2>Вход в аккаунт</h2>
      <form @submit.prevent="handleSubmit">
        <div class="form-group">
          <label for="email">Email</label>
          <InputText
            id="email"
            v-model="email"
            type="text"
            placeholder="Введите ваш email"
            :class="{ 'p-invalid': v$.email.$invalid && v$.email.$dirty }"
          />
          <small v-if="v$.email.$error" class="p-error">
            {{ v$.email.$errors[0].$message }}
          </small>
        </div>

        <div class="form-group">
          <label for="password">Пароль</label>
          <Password
            id="password"
            v-model="password"
            placeholder="Введите ваш пароль"
            :feedback="false"
            toggleMask
            :class="{ 'p-invalid': v$.password.$invalid && v$.password.$dirty }"
          />
          <small v-if="v$.password.$error" class="p-error">
            {{ v$.password.$errors[0].$message }}
          </small>
        </div>

        <div class="remember-me">
          <Checkbox v-model="rememberMe" inputId="rememberMe" :binary="true" />
          <label for="rememberMe" class="checkbox-label">Запомнить меня</label>
        </div>

        <div class="form-actions">
          <Button type="submit" label="Войти" :loading="isLoading" :disabled="isLoading" />
        </div>

        <div v-if="errorMessage" class="error-message">
          {{ errorMessage }}
        </div>

        <div class="auth-link">
          Нет аккаунта? <RouterLink to="/register">Зарегистрироваться</RouterLink>
        </div>
      </form>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { useVuelidate } from '@vuelidate/core'
import { required, email as emailValidator, helpers } from '@vuelidate/validators'
import InputText from 'primevue/inputtext'
import Password from 'primevue/password'
import Button from 'primevue/button'
import Checkbox from 'primevue/checkbox'
import { login } from '../services/auth.service'

const router = useRouter()

// Form data
const email = ref('')
const password = ref('')
const rememberMe = ref(false)
const isLoading = ref(false)
const errorMessage = ref('')

// Validation rules
const rules = {
  email: {
    required: helpers.withMessage('Введите email', required),
    email: helpers.withMessage('Введите корректный email', emailValidator),
  },
  password: {
    required: helpers.withMessage('Введите пароль', required),
  },
}

const v$ = useVuelidate(rules, { email, password })

async function handleSubmit() {
  errorMessage.value = ''

  const result = await v$.value.$validate()
  if (!result) {
    return
  }

  isLoading.value = true

  try {
    const response = await login({
      email: email.value,
      password: password.value,
      rememberMe: rememberMe.value,
    })

    if (response.success) {
      window.dispatchEvent(new CustomEvent('auth:login-success'))
      router.push('/')
    } else {
      // Handle specific error from the response if available
      if (response.error?.response?.data?.message) {
        errorMessage.value = response.error.response.data.message
      } else if (response.error?.response?.status === 401) {
        errorMessage.value = 'Неверный email или пароль. Попробуйте еще раз.'
      } else {
        errorMessage.value = 'Не удалось войти. Пожалуйста, попробуйте еще раз.'
      }
    }
  } catch (error) {
    // This will be hit if there's a network error or other exception
    errorMessage.value = 'Произошла ошибка при входе. Проверьте подключение к интернету.'
    console.error(error)
  } finally {
    isLoading.value = false
  }
}
</script>

<style scoped>
.login-container {
  display: flex;
  justify-content: center;
  align-items: center;
  min-height: 80vh;
  padding: 2rem;
}

.auth-card {
  background: white;
  border-radius: 12px;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
  padding: 2rem;
  width: 100%;
  max-width: 480px;
}

h2 {
  margin-bottom: 1.5rem;
  text-align: center;
  color: var(--primary-color);
  font-size: 1.5rem;
  font-weight: 600;
}

.form-group {
  margin-bottom: 1.5rem;
}

label {
  display: block;
  margin-bottom: 0.5rem;
  font-weight: 500;
  color: #333;
}

.p-inputtext,
.p-password {
  width: 100%;
}

/* Ensure all input fields have the same width */
:deep(.p-password),
:deep(.p-inputtext),
:deep(.p-password-input) {
  width: 100% !important;
  border-radius: 8px;
}

.remember-me {
  display: flex;
  align-items: center;
  margin-bottom: 1.5rem;
}

.checkbox-label {
  margin-left: 0.5rem;
  margin-bottom: 0;
  cursor: pointer;
}

.form-actions {
  margin-top: 1.5rem;
}

.p-button {
  width: 100%;
  border-radius: 8px;
}

.error-message {
  margin-top: 1rem;
  padding: 0.75rem;
  background-color: rgba(244, 67, 54, 0.1);
  border-radius: 4px;
  color: var(--red-600);
  text-align: center;
}

.auth-link {
  margin-top: 1.5rem;
  text-align: center;
  font-size: 0.95rem;
}

.auth-link a {
  color: var(--primary-color);
  text-decoration: none;
  font-weight: 500;
}

.auth-link a:hover {
  text-decoration: underline;
}

@media (max-width: 768px) {
  .login-container {
    padding: 1rem;
  }

  .auth-card {
    padding: 1.5rem;
  }
}
</style>
