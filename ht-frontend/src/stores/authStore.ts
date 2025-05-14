import { defineStore } from 'pinia'
import { ref, computed } from 'vue'

export const useAuthStore = defineStore('auth', () => {
  const jwtToken = ref(localStorage.getItem('jwt_token') || '')
  const isLoggedIn = computed(() => Boolean(jwtToken.value))

  function setToken(token: string) {
    jwtToken.value = token
    localStorage.setItem('jwt_token', token)
  }

  function clearToken() {
    jwtToken.value = ''
    localStorage.removeItem('jwt_token')
  }

  return {
    jwtToken,
    isLoggedIn,
    setToken,
    clearToken,
  }
})
