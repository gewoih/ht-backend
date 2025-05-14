<template>
  <div class="login-container">
    <div class="login-card">
      <h2>Sign In</h2>
      <form @submit.prevent="handleSubmit">
        <div class="form-group">
          <label for="email">Email</label>
          <InputText
            id="email"
            v-model="email"
            type="text"
            placeholder="Enter your email"
            :class="{ 'p-invalid': v$.email.$invalid && v$.email.$dirty }"
          />
          <small v-if="v$.email.$error" class="p-error">
            {{ v$.email.$errors[0].$message }}
          </small>
        </div>

        <div class="form-group">
          <label for="password">Password</label>
          <Password
            id="password"
            v-model="password"
            placeholder="Enter your password"
            :feedback="false"
            toggleMask
            :class="{ 'p-invalid': v$.password.$invalid && v$.password.$dirty }"
          />
          <small v-if="v$.password.$error" class="p-error">
            {{ v$.password.$errors[0].$message }}
          </small>
        </div>

        <div class="form-actions">
          <Button type="submit" label="Sign In" :loading="isLoading" :disabled="isLoading" />
        </div>

        <div v-if="errorMessage" class="error-message">
          {{ errorMessage }}
        </div>

        <div class="register-link">
          Don't have an account? <RouterLink to="/register">Register</RouterLink>
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
import { login } from '../services/auth.service'

const router = useRouter()

// Form data
const email = ref('')
const password = ref('')
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
    })

    if (response.success) {
      window.dispatchEvent(new CustomEvent('auth:login-success'))
      router.push('/')
    } else {
      // Handle specific error from the response if available
      if (response.error?.response?.data?.message) {
        errorMessage.value = response.error.response.data.message
      } else if (response.error?.response?.status === 401) {
        errorMessage.value = 'Invalid email or password. Please try again.'
      } else {
        errorMessage.value = 'Login failed. Please try again.'
      }
    }
  } catch (error) {
    // This will be hit if there's a network error or other exception
    errorMessage.value = 'An error occurred during login. Please check your connection.'
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

.login-card {
  background: white;
  border-radius: 8px;
  box-shadow: 0 4px 10px rgba(0, 0, 0, 0.1);
  padding: 2rem;
  width: 100%;
  max-width: 480px;
}

h2 {
  margin-bottom: 1.5rem;
  text-align: center;
  color: var(--primary-color);
}

.form-group {
  margin-bottom: 1.5rem;
}

label {
  display: block;
  margin-bottom: 0.5rem;
  font-weight: 500;
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
}

.form-actions {
  margin-top: 1.5rem;
}

.p-button {
  width: 100%;
}

.error-message {
  margin-top: 1rem;
  color: var(--red-500);
  text-align: center;
}

.register-link {
  margin-top: 1.5rem;
  text-align: center;
}

.register-link a {
  color: var(--primary-color);
  text-decoration: none;
  font-weight: 500;
}

.register-link a:hover {
  text-decoration: underline;
}
</style>
