<template>
  <div class="auth-container">
    <div class="auth-card">
      <h2>Создание аккаунта</h2>
      <form @submit.prevent="handleSubmit">
        <div class="form-group">
          <label for="username">Имя пользователя</label>
          <InputText
            id="username"
            v-model="username"
            type="text"
            placeholder="Введите имя пользователя"
            :class="{ 'p-invalid': v$.username.$invalid && v$.username.$dirty }"
          />
          <small v-if="v$.username.$error" class="p-error">
            {{ v$.username.$errors[0].$message }}
          </small>
        </div>

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
          <PasswordField
            id="password"
            v-model="password"
            placeholder="Создайте пароль"
            :invalid="v$.password.$invalid && v$.password.$dirty"
          />
          <small v-if="v$.password.$error" class="p-error">
            {{ v$.password.$errors[0].$message }}
          </small>
        </div>

        <div class="form-group">
          <label for="confirmPassword">Подтверждение пароля</label>
          <PasswordField
            id="confirmPassword"
            v-model="confirmPassword"
            placeholder="Подтвердите пароль"
            :invalid="v$.confirmPassword.$invalid && v$.confirmPassword.$dirty"
          />
          <small v-if="v$.confirmPassword.$error" class="p-error">
            {{ v$.confirmPassword.$errors[0].$message }}
          </small>
        </div>

        <div class="form-actions">
          <Button
            type="submit"
            label="Зарегистрироваться"
            :loading="isLoading"
            :disabled="isLoading"
          />
        </div>

        <div v-if="errorMessage" class="error-message">
          {{ errorMessage }}
        </div>

        <div class="auth-link">Уже есть аккаунт? <RouterLink to="/login">Войти</RouterLink></div>
      </form>
    </div>
  </div>

  <!-- Email verification dialog -->
  <Dialog
    v-model:visible="showVerificationDialog"
    modal
    header="Подтверждение email"
    :closable="false"
    class="verification-dialog"
    :breakpoints="{ '960px': '80vw', '640px': '90vw' }"
    :style="{ width: '400px' }"
    :pt="{ root: { style: 'max-height: 90vh; overflow-y: auto' } }"
  >
    <div class="verification-container">
      <div class="verification-icon">
        <i class="pi pi-envelope"></i>
      </div>

      <h3 class="verification-title">Проверьте ваш почтовый ящик</h3>

      <p class="verification-text">
        Мы отправили код подтверждения на <strong>{{ email }}</strong
        >. Введите 6-значный код для завершения регистрации.
      </p>

      <div v-if="isSendingCode" class="sending-code-indicator">
        <i class="pi pi-spin pi-spinner"></i>
        <span>Отправка кода...</span>
      </div>

      <div class="code-input-container" @paste="handlePaste">
        <div v-for="(digit, index) in 6" :key="index" class="code-digit-wrapper">
          <InputText
            :ref="
              (el) => {
                if (el) codeInputRefs[index] = el
              }
            "
            v-model="codeDigits[index]"
            class="code-digit-input"
            maxlength="1"
            @input="handleCodeDigitInput(index)"
            @keydown="handleCodeDigitKeydown($event, index)"
            :class="{ 'p-invalid': verificationCodeError }"
          />
        </div>
      </div>

      <small v-if="verificationCodeError" class="p-error verification-error">
        {{ verificationCodeError }}
      </small>

      <div class="verification-actions">
        <Button
          label="Подтвердить"
          @click="confirmEmailVerification"
          :loading="isVerifying"
          :disabled="isVerifying || !isVerificationCodeComplete()"
          class="verify-button"
        />
      </div>

      <div class="resend-code">
        Не получили код?
        <a href="#" @click.prevent="resendCode" :class="{ disabled: resendDisabled }">
          {{ resendDisabled ? `Отправить повторно (${resendCountdown}с)` : 'Отправить повторно' }}
        </a>
      </div>
    </div>
  </Dialog>
</template>

<script setup lang="ts">
import { ref, computed, onMounted, onBeforeUnmount } from 'vue'
import { useRouter } from 'vue-router'
import { useVuelidate } from '@vuelidate/core'
import { required, email as emailValidator, sameAs, helpers } from '@vuelidate/validators'
import InputText from 'primevue/inputtext'
import PasswordField from '../components/ui/PasswordField.vue'
import Button from 'primevue/button'
import Dialog from 'primevue/dialog'
import { register, confirmEmail, sendEmailConfirmation } from '../services/auth.service'
import { useAuthStore } from '../stores/authStore'

const router = useRouter()
const authStore = useAuthStore()

// Form data
const username = ref('')
const email = ref('')
const password = ref('')
const confirmPassword = ref('')
const isLoading = ref(false)
const errorMessage = ref('')

// Verification dialog
const showVerificationDialog = ref(false)
const verificationCodeError = ref('')
const isVerifying = ref(false)
const codeDigits = ref(['', '', '', '', '', ''])
const codeInputRefs = ref<any[]>([])
const isSendingCode = ref(false)
const resendDisabled = ref(false)
const resendCountdown = ref(0)
const resendTimer = ref<number | null>(null)

// Validation rules
const rules = {
  username: {
    required: helpers.withMessage('Необходимо указать имя пользователя', required),
  },
  email: {
    required: helpers.withMessage('Необходимо указать email', required),
    email: helpers.withMessage('Пожалуйста, введите корректный email адрес', emailValidator),
  },
  password: {
    required: helpers.withMessage('Необходимо указать пароль', required),
  },
  confirmPassword: {
    required: helpers.withMessage('Пожалуйста, подтвердите пароль', required),
    sameAsPassword: helpers.withMessage(
      'Пароли должны совпадать',
      (value) => value === password.value
    ),
  },
}

const v$ = useVuelidate(rules, { username, email, password, confirmPassword })

// Handle code input
function handleCodeDigitInput(index: number) {
  // Only allow numeric inputs
  codeDigits.value[index] = codeDigits.value[index].replace(/[^0-9]/g, '')

  // Move to next input when digit is entered
  if (codeDigits.value[index] && index < 5) {
    codeInputRefs.value[index + 1]?.focus()
  }

  // Clear error when user is typing
  verificationCodeError.value = ''

  // If all digits are filled, check if we should submit
  if (isVerificationCodeComplete() && index === 5) {
    confirmEmailVerification()
  }
}

function handleCodeDigitKeydown(event: KeyboardEvent, index: number) {
  // Handle backspace - move to previous input when current is empty
  if (event.key === 'Backspace' && !codeDigits.value[index] && index > 0) {
    codeInputRefs.value[index - 1]?.focus()
  }

  // Handle arrow keys for navigation
  if (event.key === 'ArrowLeft' && index > 0) {
    event.preventDefault()
    codeInputRefs.value[index - 1]?.focus()
  }

  if (event.key === 'ArrowRight' && index < 5) {
    event.preventDefault()
    codeInputRefs.value[index + 1]?.focus()
  }
}

function isVerificationCodeComplete() {
  return codeDigits.value.every((digit) => digit.trim() !== '')
}

function getVerificationCode() {
  return codeDigits.value.join('')
}

async function handleSubmit() {
  errorMessage.value = ''

  const result = await v$.value.$validate()
  if (!result) {
    return
  }

  isLoading.value = true

  try {
    const response = await register({
      username: username.value,
      email: email.value,
      password: password.value,
    })

    if (response.success) {
      // Show verification dialog first
      showVerificationDialog.value = true

      // Then send confirmation email
      await sendCodeToEmail()

      // Focus first code input field
      setTimeout(() => {
        if (codeInputRefs.value[0]) {
          codeInputRefs.value[0].focus()
        }
      }, 100)
    } else {
      errorMessage.value = 'Ошибка регистрации. Пожалуйста, попробуйте еще раз.'
    }
  } catch (error) {
    errorMessage.value = 'Произошла ошибка при регистрации.'
    console.error(error)
  } finally {
    isLoading.value = false
  }
}

async function sendCodeToEmail() {
  isSendingCode.value = true
  verificationCodeError.value = ''

  try {
    const response = await sendEmailConfirmation(email.value)
    if (!response.success) {
      verificationCodeError.value = 'Не удалось отправить код подтверждения'
    } else {
      startResendCooldown()
    }
  } catch (error) {
    verificationCodeError.value = 'Ошибка при отправке кода подтверждения'
    console.error(error)
  } finally {
    isSendingCode.value = false
  }
}

function startResendCooldown() {
  // Disable resend button for 60 seconds
  resendDisabled.value = true
  resendCountdown.value = 60

  if (resendTimer.value) {
    clearInterval(resendTimer.value)
  }

  resendTimer.value = window.setInterval(() => {
    resendCountdown.value--
    if (resendCountdown.value <= 0) {
      resendDisabled.value = false
      if (resendTimer.value) {
        clearInterval(resendTimer.value)
        resendTimer.value = null
      }
    }
  }, 1000)
}

async function resendCode() {
  if (resendDisabled.value) return
  await sendCodeToEmail()
}

async function confirmEmailVerification() {
  const verificationCode = getVerificationCode()

  if (!verificationCode || verificationCode.length !== 6) {
    verificationCodeError.value = 'Пожалуйста, введите 6-значный код'
    return
  }

  verificationCodeError.value = ''
  isVerifying.value = true

  try {
    const response = await confirmEmail(verificationCode, email.value)

    if (response.success) {
      showVerificationDialog.value = false
      router.push('/login')
    } else {
      verificationCodeError.value = 'Неверный код подтверждения'
    }
  } catch (error) {
    verificationCodeError.value = 'Произошла ошибка при подтверждении'
    console.error(error)
  } finally {
    isVerifying.value = false
  }
}

function handlePaste(event: ClipboardEvent) {
  event.preventDefault()
  // Handle paste event
  const pasteData = event.clipboardData?.getData('text')
  if (!pasteData) return

  // Filter to only include numbers
  const digits = pasteData
    .replace(/[^0-9]/g, '')
    .split('')
    .slice(0, 6)

  // Fill in the available digits
  digits.forEach((digit, index) => {
    if (index < 6) {
      codeDigits.value[index] = digit
    }
  })

  // Focus the next empty input or the last one if all filled
  const nextEmptyIndex = codeDigits.value.findIndex((digit) => !digit)
  if (nextEmptyIndex !== -1) {
    codeInputRefs.value[nextEmptyIndex]?.focus()
  } else if (digits.length > 0) {
    codeInputRefs.value[Math.min(5, digits.length - 1)]?.focus()

    // If all fields are filled, try to submit
    if (isVerificationCodeComplete()) {
      setTimeout(() => confirmEmailVerification(), 100)
    }
  }

  // Clear error
  verificationCodeError.value = ''
}

// Clean up function when component is unmounted
onBeforeUnmount(() => {
  if (resendTimer.value) {
    clearInterval(resendTimer.value)
  }
})
</script>

<style scoped>
/* Empty style block - all styles are imported from auth.css */
</style>
