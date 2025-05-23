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
          <small v-if="isUsernameNonLatin" class="p-error">
            Имя пользователя должно содержать только латинские буквы, цифры, "_" или "-"
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
            @input="v$.email.$touch()"
          />
          <small
            v-if="
              (v$.email.$dirty && v$.email.$invalid && v$.email.$errors.length) ||
              (email.length > 0 && v$.email.$errors.length)
            "
            class="p-error"
          >
            {{ v$.email.$errors[0].$message }}
          </small>
          <small v-else-if="isEmailNonLatin" class="p-error">
            Email должен содержать только латинские буквы, цифры и символы @ . _ - +
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
          <ul class="password-rules-list">
            <li :class="{ passed: isPasswordMinLength }">
              <i
                :class="isPasswordMinLength ? 'pi pi-check-circle passed' : 'pi pi-times-circle'"
              />
              Не менее 8 символов
            </li>
            <li :class="{ passed: isPasswordUniqueChars }">
              <i
                :class="isPasswordUniqueChars ? 'pi pi-check-circle passed' : 'pi pi-times-circle'"
              />
              Минимум 4 уникальных символа
            </li>
            <li :class="{ passed: isPasswordLowerUpper }">
              <i
                :class="isPasswordLowerUpper ? 'pi pi-check-circle passed' : 'pi pi-times-circle'"
              />
              Строчная и заглавная буква
            </li>
            <li :class="{ passed: isPasswordDigit }">
              <i :class="isPasswordDigit ? 'pi pi-check-circle passed' : 'pi pi-times-circle'" />
              Хотя бы одна цифра
            </li>
          </ul>
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
            :disabled="isLoading || !isFormValid"
          />
        </div>

        <div v-if="errorMessage" class="error-message">
          {{ errorMessage }}
        </div>

        <div class="auth-link">Уже есть аккаунт? <RouterLink to="/login">Войти</RouterLink></div>
      </form>
    </div>
  </div>

  <!-- Email verification dialog extracted to a component -->
  <EmailVerificationDialog
    v-model:visible="showVerificationDialog"
    :email="email"
    :is-sending-code="isSendingCode"
    :is-verifying="isVerifying"
    :verification-code-error="verificationCodeError"
    :resend-disabled="resendDisabled"
    :resend-countdown="resendCountdown"
    :code-digits="codeDigits"
    @update:code-digits="
      (val) => {
        if (JSON.stringify(codeDigits) !== JSON.stringify(val)) codeDigits = val
      }
    "
    @confirm="confirmEmailVerification"
    @resend="resendCode"
  />
</template>

<script setup lang="ts">
import { ref, onBeforeUnmount, computed } from 'vue'
import { useRouter } from 'vue-router'
import { useVuelidate } from '@vuelidate/core'
import { required, email as emailValidator, helpers } from '@vuelidate/validators'
import InputText from 'primevue/inputtext'
import PasswordField from '../components/ui/PasswordField.vue'
import Button from 'primevue/button'
import { register, confirmEmail, sendEmailConfirmation } from '../services/auth.service'
import EmailVerificationDialog from '../components/ui/EmailVerificationDialog.vue'
import 'primeicons/primeicons.css'

const router = useRouter()

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

// Custom password validators
function minLength8(value: string) {
  return value.length >= 8
}
function minUniqueChars4(value: string) {
  return new Set(value).size >= 4
}
function hasLowerAndUpper(value: string) {
  return /[a-z]/.test(value) && /[A-Z]/.test(value)
}
function hasDigit(value: string) {
  return /\d/.test(value)
}

// Custom validators
function isLatinOnly(value: string) {
  // Allows only Latin letters, numbers, underscores, and dashes
  return /^[A-Za-z0-9_-]+$/.test(value)
}

function isLatinEmail(value: string) {
  // Allows only Latin letters, numbers, and common email symbols
  return /^[A-Za-z0-9@._\-+]+$/.test(value)
}

// Validation rules
const rules = {
  username: {
    required: helpers.withMessage('Необходимо указать имя пользователя', required),
    isLatinOnly: helpers.withMessage(
      'Имя пользователя должно содержать только латинские буквы, цифры, "_" или "-"',
      isLatinOnly
    ),
  },
  email: {
    required: helpers.withMessage('Необходимо указать email', required),
    email: helpers.withMessage('Пожалуйста, введите корректный email адрес', emailValidator),
  },
  password: {
    required: helpers.withMessage('Необходимо указать пароль', required),
    minLength8: helpers.withMessage('Пароль должен содержать не менее 8 символов', minLength8),
    minUniqueChars4: helpers.withMessage(
      'Пароль должен содержать минимум 4 уникальных символа',
      minUniqueChars4
    ),
    hasLowerAndUpper: helpers.withMessage(
      'Пароль должен содержать хотя бы одну строчную и одну заглавную букву',
      hasLowerAndUpper
    ),
    hasDigit: helpers.withMessage('Пароль должен содержать хотя бы одну цифру', hasDigit),
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

const isPasswordMinLength = computed(() => minLength8(password.value))
const isPasswordUniqueChars = computed(() => minUniqueChars4(password.value))
const isPasswordLowerUpper = computed(() => hasLowerAndUpper(password.value))
const isPasswordDigit = computed(() => hasDigit(password.value))

const isUsernameNonLatin = computed(() => {
  return username.value.length > 0 && !isLatinOnly(username.value)
})

const isEmailNonLatin = computed(() => {
  return email.value.length > 0 && !isLatinEmail(email.value)
})

function isVerificationCodeComplete() {
  return codeDigits.value.every((digit) => digit.trim() !== '')
}

function getVerificationCode() {
  return codeDigits.value.join('')
}

const isFormValid = computed(() => {
  return !v$.value.$invalid && !isUsernameNonLatin.value && !isEmailNonLatin.value
})

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
      if (codeInputRefs.value[0]) {
        codeInputRefs.value[0].focus()
      }
    } else {
      if (response.error?.response?.status === 400) {
        errorMessage.value = 'Аккаунт с таким email или логином уже существует'
      } else {
        errorMessage.value = 'Произошла ошибка при регистрации. Попробуйте позже.'
      }
    }
  } catch (error) {
    errorMessage.value = 'Произошла ошибка при регистрации. Попробуйте позже.'
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
      confirmEmailVerification()
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
.password-rules-list {
  list-style: none;
  margin: 0.5rem 0 0 0;
  padding: 0;
  font-size: 0.97rem;
}
.password-rules-list li {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  color: #888;
  margin-bottom: 0.2rem;
  transition: color 0.2s;
}
.password-rules-list li.passed {
  color: #22c55e;
}
.password-rules-list i {
  font-size: 1.1rem;
  min-width: 1.2rem;
}
.password-rules-list i.passed {
  color: #22c55e;
}
</style>
