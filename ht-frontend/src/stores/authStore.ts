import { defineStore } from 'pinia'
import { ref, computed } from 'vue'

export const useAuthStore = defineStore('auth', () => {
  // Get initial token from storage
  const getInitialToken = (): string => {
    return localStorage.getItem('access_token') || sessionStorage.getItem('access_token') || ''
  }

  const jwtToken = ref(getInitialToken())
  const isLoggedIn = computed(() => Boolean(jwtToken.value))

  // Track where the token is stored (for persistence preference)
  const useLocalStorage = ref(Boolean(localStorage.getItem('access_token')))

  function setToken(token: string, rememberMe: boolean = false) {
    jwtToken.value = token

    // Store based on remember me preference
    if (rememberMe) {
      localStorage.setItem('access_token', token)
      useLocalStorage.value = true
    } else {
      sessionStorage.setItem('access_token', token)
      useLocalStorage.value = false
    }
  }

  function clearToken() {
    jwtToken.value = ''
    localStorage.removeItem('access_token')
    sessionStorage.removeItem('access_token')
  }

  return {
    jwtToken,
    isLoggedIn,
    setToken,
    clearToken,
    useLocalStorage,
  }
})
