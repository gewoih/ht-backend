import { useRouter } from 'vue-router'
import http from './http.service'
import { API_ENDPOINTS } from '../config/api'
import { log } from 'console'

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
    console.log(response.data)
    const token = response.data
    localStorage.setItem('jwt_token', token)
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

export const logout = () => {
  localStorage.removeItem('jwt_token')

  try {
    const router = useRouter()
    router.push('/')
  } catch {
    window.location.href = '/'
  }
}

export const isAuthenticated = () => {
  return !!localStorage.getItem('jwt_token')
}
