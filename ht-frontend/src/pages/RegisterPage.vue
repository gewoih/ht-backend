<template>
  <div class="register-container">
    <div class="register-card">
      <h2>Create an Account</h2>
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
            placeholder="Create a password"
            :feedback="false"
            toggleMask
            :class="{ 'p-invalid': v$.password.$invalid && v$.password.$dirty }"
          />
          <small v-if="v$.password.$error" class="p-error">
            {{ v$.password.$errors[0].$message }}
          </small>
        </div>

        <div class="form-group">
          <label for="confirmPassword">Confirm Password</label>
          <Password
            id="confirmPassword"
            v-model="confirmPassword"
            placeholder="Confirm your password"
            :feedback="false"
            toggleMask
            :class="{ 'p-invalid': v$.confirmPassword.$invalid && v$.confirmPassword.$dirty }"
          />
          <small v-if="v$.confirmPassword.$error" class="p-error">
            {{ v$.confirmPassword.$errors[0].$message }}
          </small>
        </div>

        <div class="form-actions">
          <Button type="submit" label="Register" :loading="isLoading" :disabled="isLoading" />
        </div>

        <div v-if="errorMessage" class="error-message">
          {{ errorMessage }}
        </div>

        <div class="login-link">
          Already have an account? <RouterLink to="/login">Log in</RouterLink>
        </div>
      </form>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { useVuelidate } from '@vuelidate/core'
import { required, email as emailValidator, sameAs, helpers } from '@vuelidate/validators'
import InputText from 'primevue/inputtext'
import Password from 'primevue/password'
import Button from 'primevue/button'
import { register } from '../services/auth.service'
import { useAuthStore } from '../stores/authStore'

const router = useRouter()
const authStore = useAuthStore()

// Form data
const email = ref('')
const password = ref('')
const confirmPassword = ref('')
const isLoading = ref(false)
const errorMessage = ref('')

// Validation rules
const rules = {
  email: {
    required: helpers.withMessage('Email is required', required),
    email: helpers.withMessage('Please enter a valid email address', emailValidator),
  },
  password: {
    required: helpers.withMessage('Password is required', required),
  },
  confirmPassword: {
    required: helpers.withMessage('Please confirm your password', required),
    sameAsPassword: helpers.withMessage(
      'Passwords must match',
      (value) => value === password.value
    ),
  },
}

const v$ = useVuelidate(rules, { email, password, confirmPassword })

async function handleSubmit() {
  errorMessage.value = ''

  const result = await v$.value.$validate()
  if (!result) {
    return
  }

  isLoading.value = true

  try {
    const response = await register({
      email: email.value,
      password: password.value,
    })

    if (response.success) {
      router.push('/login')
    } else {
      errorMessage.value = 'Registration failed. Please try again.'
    }
  } catch (error) {
    errorMessage.value = 'An error occurred during registration.'
    console.error(error)
  } finally {
    isLoading.value = false
  }
}
</script>

<style scoped>
.register-container {
  display: flex;
  justify-content: center;
  align-items: center;
  min-height: 80vh;
  padding: 2rem;
}

.register-card {
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

.login-link {
  margin-top: 1.5rem;
  text-align: center;
}

.login-link a {
  color: var(--primary-color);
  text-decoration: none;
  font-weight: 500;
}

.login-link a:hover {
  text-decoration: underline;
}
</style>
