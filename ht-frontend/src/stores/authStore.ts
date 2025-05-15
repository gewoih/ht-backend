import { defineStore } from 'pinia'
import { ref, computed } from 'vue'

export const useAuthStore = defineStore('auth', () => {
  const jwtToken = ref(sessionStorage.getItem('access_token') || '')
  const isLoggedIn = computed(() => Boolean(jwtToken.value))

  function setToken(token: string) {
    jwtToken.value = token
    sessionStorage.setItem('access_token', token)
  }

  function clearToken() {
    jwtToken.value = ''
    sessionStorage.removeItem('access_token')
  }

  return {
    jwtToken,
    isLoggedIn,
    setToken,
    clearToken,
  }
})
