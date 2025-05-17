import axios, { AxiosRequestConfig } from 'axios'
import { API_ENDPOINTS } from '../config/api'
import { useAuthStore } from '../stores/authStore'

const API_BASE_URL = (import.meta as any).env.VITE_API_BASE_URL

interface RetryConfig extends AxiosRequestConfig {
  _retry?: boolean
}

const http = axios.create({
  baseURL: API_BASE_URL,
  withCredentials: true,
  headers: { 'Content-Type': 'application/json' },
})

http.interceptors.request.use((cfg) => {
  const authStore = useAuthStore()
  const token = authStore.jwtToken
  if (token) cfg.headers.Authorization = `Bearer ${token}`
  return cfg
})

let isRefreshing = false
let queue: Array<(t: string) => void> = []

http.interceptors.response.use(
  (r) => r,
  async (err) => {
    const original = err.config as RetryConfig
    if (err.response?.status === 401 && !original._retry) {
      original._retry = true

      if (isRefreshing) {
        return new Promise((resolve) =>
          queue.push((token) => {
            if (!original.headers) original.headers = {}
            original.headers.Authorization = `Bearer ${token}`
            resolve(http(original))
          })
        )
      }

      isRefreshing = true
      const authStore = useAuthStore()
      try {
        const { data } = await axios.post(
          `${API_BASE_URL}${API_ENDPOINTS.auth.refresh}`,
          {},
          { withCredentials: true }
        )
        const { accessToken } = data
        authStore.setToken(accessToken, authStore.useLocalStorage)

        queue.forEach((cb) => cb(accessToken))
        queue = []

        if (!original.headers) original.headers = {}
        original.headers.Authorization = `Bearer ${accessToken}`
        return http(original)
      } catch (e) {
        authStore.clearToken()
        queue = []
        return Promise.reject(e)
      } finally {
        isRefreshing = false
      }
    }
    return Promise.reject(err)
  }
)

export default http
