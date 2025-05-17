<template>
  <div
    class="p-password p-component p-inputwrapper"
    :class="{ 'p-inputwrapper-filled': modelValue }"
  >
    <input
      :id="id"
      ref="input"
      :type="visible ? 'text' : 'password'"
      :class="['p-password-input p-inputtext p-component', { 'p-invalid': invalid }]"
      :value="modelValue"
      @input="handleInput"
      :placeholder="placeholder"
      :disabled="disabled"
    />
    <button class="p-password-toggle p-link" type="button" @click="toggleVisibility" tabindex="-1">
      <i class="password-icon" :class="visible ? 'pi pi-eye' : 'pi pi-eye-slash'"></i>
    </button>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'

const props = defineProps({
  modelValue: {
    type: String,
    required: true,
  },
  id: {
    type: String,
    default: undefined,
  },
  placeholder: {
    type: String,
    default: '',
  },
  disabled: {
    type: Boolean,
    default: false,
  },
  invalid: {
    type: Boolean,
    default: false,
  },
})

const emit = defineEmits(['update:modelValue'])

const visible = ref(false)
const input = ref<HTMLInputElement | null>(null)

function toggleVisibility() {
  visible.value = !visible.value
}

function handleInput(event: Event) {
  const target = event.target as HTMLInputElement
  emit('update:modelValue', target.value)
}
</script>

<style scoped>
.p-password {
  position: relative;
  display: inline-flex;
  width: 100%;
}

.p-password-input {
  width: 100%;
  padding-right: 2.5rem;
}

.p-password-toggle {
  position: absolute;
  top: 0;
  right: 0;
  bottom: 0;
  display: flex;
  align-items: center;
  justify-content: center;
  border: none;
  background: transparent;
  cursor: pointer;
  padding: 0 0.75rem;
}

.password-icon {
  color: #6c757d;
}
</style>
