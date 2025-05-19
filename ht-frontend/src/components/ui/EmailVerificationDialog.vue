<template>
  <Dialog
    :visible="visible"
    @update:visible="(val) => emit('update:visible', val)"
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
      <div class="code-input-container">
        <div v-for="(_, index) in 6" :key="index" class="code-digit-wrapper">
          <InputText
            :ref="(el) => setCodeInputRef(el, index)"
            v-model="localCodeDigits[index]"
            class="code-digit-input"
            maxlength="1"
            inputmode="numeric"
            pattern="[0-9]*"
            @input="(e) => handleInput(e, index)"
            @keydown="handleCodeDigitKeydown($event, index)"
            @paste="onPaste"
            :class="{ 'p-invalid': verificationCodeError }"
            autocomplete="one-time-code"
          />
        </div>
      </div>

      <small v-if="verificationCodeError" class="p-error verification-error">
        {{ verificationCodeError }}
      </small>
      <div class="verification-actions">
        <Button
          label="Подтвердить"
          @click="emitConfirm"
          :loading="isVerifying"
          :disabled="isVerifying || !isVerificationCodeComplete"
          class="verify-button"
        />
      </div>
      <div class="resend-code">
        Не получили код?
        <a href="#" @click.prevent="emitResend" :class="{ disabled: resendDisabled }">
          {{ resendDisabled ? `Отправить повторно (${resendCountdown}с)` : 'Отправить повторно' }}
        </a>
      </div>
    </div>
  </Dialog>
</template>

<script setup lang="ts">
import { ref, watch, computed, nextTick } from 'vue'
import InputText from 'primevue/inputtext'
import Button from 'primevue/button'
import Dialog from 'primevue/dialog'

interface Props {
  visible: boolean
  email: string
  isSendingCode: boolean
  isVerifying: boolean
  verificationCodeError: string
  resendDisabled: boolean
  resendCountdown: number
  codeDigits: string[]
}

const props = defineProps<Props>()
const emit = defineEmits<{
  (e: 'update:visible', value: boolean): void
  (e: 'update:code-digits', value: string[]): void
  (e: 'confirm'): void
  (e: 'resend'): void
}>()

const localCodeDigits = ref([...props.codeDigits])
const codeInputRefs = ref<Array<HTMLInputElement | null>>(Array(6).fill(null))

watch(
  () => props.codeDigits,
  (val) => {
    localCodeDigits.value = [...val]
  }
)

watch(
  () => localCodeDigits.value,
  (val) => {
    if (JSON.stringify(val) !== JSON.stringify(props.codeDigits)) {
      emit('update:code-digits', [...val])
    }
  },
  { deep: true }
)

// true, когда все 6 ячеек заполнены
const isVerificationCodeComplete = computed(() =>
  localCodeDigits.value.every((d) => d.trim() !== '')
)

// как только код заполнен и мы не в процессе верификации — эмитим confirm
watch(isVerificationCodeComplete, (complete) => {
  if (complete && !props.isVerifying) {
    nextTick(() => emitConfirm())
  }
})

function setCodeInputRef(el: any, index: number) {
  // For PrimeVue InputText, el is a Vue component instance
  if (el && el.$el) {
    // Try to get the native input element
    const input = el.$el instanceof HTMLInputElement ? el.$el : el.$el.querySelector('input')
    codeInputRefs.value[index] = input
  }
}

function handleInput(event: Event, index: number) {
  const input = event.target as HTMLInputElement
  let value = input.value.replace(/\D/g, '')
  // If user pasted or typed multiple digits, distribute
  if (value.length > 1) {
    for (let i = 0; i < 6; i++) {
      localCodeDigits.value[i] = value[i] || ''
    }
    nextTick(() => {
      const nextEmpty = localCodeDigits.value.findIndex((d) => !d)
      if (nextEmpty >= 0) codeInputRefs.value[nextEmpty]?.focus()
    })
    return
  }
  // Single digit input
  localCodeDigits.value[index] = value
  if (value && index < 5) {
    nextTick(() => {
      const nextEmpty = localCodeDigits.value.findIndex((d, i) => !d && i > index)
      const target = nextEmpty !== -1 ? nextEmpty : index + 1
      codeInputRefs.value[target]?.focus()
    })
  }
}

function handleCodeDigitKeydown(event: KeyboardEvent, index: number) {
  if (event.key === 'Backspace' && !localCodeDigits.value[index] && index > 0) {
    nextTick(() => codeInputRefs.value[index - 1]?.focus())
  }
  if (event.key === 'ArrowLeft' && index > 0) {
    event.preventDefault()
    nextTick(() => codeInputRefs.value[index - 1]?.focus())
  }
  if (event.key === 'ArrowRight' && index < 5) {
    event.preventDefault()
    nextTick(() => codeInputRefs.value[index + 1]?.focus())
  }
}

function onPaste(event: ClipboardEvent) {
  event.preventDefault()
  const pasteData = event.clipboardData?.getData('text')
  if (!pasteData) return
  const digits = pasteData.replace(/\D/g, '').slice(0, 6).split('')
  for (let i = 0; i < 6; i++) {
    localCodeDigits.value[i] = digits[i] || ''
  }
  nextTick(() => {
    const nextEmpty = localCodeDigits.value.findIndex((d) => !d)
    if (nextEmpty >= 0) codeInputRefs.value[nextEmpty]?.focus()
  })
}

function emitConfirm() {
  emit('confirm')
}
function emitResend() {
  emit('resend')
}
</script>

<style scoped>
.verification-container {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 1.2rem;
  padding: 1.5rem 0.5rem 0.5rem 0.5rem;
}
.verification-icon {
  font-size: 2.5rem;
  color: #1976d2;
  margin-bottom: 0.5rem;
}
.verification-title {
  font-size: 1.2rem;
  font-weight: 600;
  color: #1976d2;
  margin-bottom: 0.2rem;
}
.verification-text {
  font-size: 1rem;
  color: #333;
  text-align: center;
  margin-bottom: 0.5rem;
}
.sending-code-indicator {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  color: #1976d2;
  font-size: 1rem;
}
.code-input-container {
  display: flex;
  gap: 0.5rem;
  justify-content: center;
  margin: 0.5rem 0 0.2rem 0;
}
.code-digit-wrapper {
  width: 2.2rem;
  height: 2.2rem;
}
.code-digit-input {
  width: 100%;
  height: 2.2rem;
  text-align: center;
  font-size: 1.3rem;
  border-radius: 6px;
  border: 1px solid #d1d5db;
  outline: none;
  transition: border 0.2s;
}
.code-digit-input:focus {
  border: 1.5px solid #1976d2;
}
.verification-error {
  color: #ef4444;
  font-size: 0.95rem;
  margin-top: 0.2rem;
}
.verification-actions {
  margin-top: 0.5rem;
  width: 100%;
  display: flex;
  justify-content: center;
}
.verify-button {
  width: 100%;
}
.resend-code {
  margin-top: 0.7rem;
  font-size: 0.97rem;
  color: #1976d2;
  text-align: center;
}
.resend-code a {
  color: #1976d2;
  text-decoration: underline;
  cursor: pointer;
  margin-left: 0.3rem;
}
.resend-code a.disabled {
  color: #aaa;
  pointer-events: none;
  text-decoration: none;
}
</style>
