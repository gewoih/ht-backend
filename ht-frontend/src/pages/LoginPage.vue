<template>
  <div class="auth-container">
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
/* Empty style block - all styles are imported from auth.css */
</style>
