import { useRouter } from 'vue-router'
import http from './http.service'
import { API_ENDPOINTS } from '../config/api'
import { useAuthStore } from '../stores/authStore'

interface LoginCredentials {
  email: string
  password: string
}

interface RegisterCredentials {
  email: string
  password: string
}

export const login = async (credentials: LoginCredentials) => {
  try {
    const response = await http.post(API_ENDPOINTS.auth.login, credentials)
    const accessToken = response.data.accessToken
    const authStore = useAuthStore()
    authStore.setToken(accessToken)
    return { success: true, data: response.data }
  } catch (error) {
    return { success: false, error }
  }
}

export const register = async (credentials: RegisterCredentials) => {
  try {
    const response = await http.post(API_ENDPOINTS.auth.register, credentials)
    return { success: true, data: response.data }
  } catch (error) {
    return { success: false, error }
  }
}

export const logout = async () => {
  try {
    // Call the logout API to invalidate the refresh token in cookies
    await http.post(
      API_ENDPOINTS.auth.logout,
      {},
      {
        withCredentials: true,
      }
    )
  } catch (error) {
    console.error('Error during logout:', error)
  } finally {
    // Clear the access token regardless of API call success
    const authStore = useAuthStore()
    authStore.clearToken()

    // Redirect to login page
    try {
      const router = useRouter()
      router.push('/login')
    } catch {
      window.location.href = '/login'
    }
  }
}

export const refreshToken = async () => {
  try {
    const response = await http.post(
      API_ENDPOINTS.auth.refresh,
      {},
      {
        withCredentials: true, // Include cookies in the request
      }
    )
    const { accessToken } = response.data
    const authStore = useAuthStore()
    authStore.setToken(accessToken)
    return { success: true, data: response.data }
  } catch (error) {
    return { success: false, error }
  }
}

export const isAuthenticated = () => {
  const authStore = useAuthStore()
  return authStore.isLoggedIn
}
