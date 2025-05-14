import axios from 'axios'

const http = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
})

// Add request interceptor to include JWT token
http.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('jwt_token')
    if (token) {
      config.headers.Authorization = `Bearer ${token}`
    }
    return config
  },
  (error) => {
    return Promise.reject(error)
  }
)

// Create a flag to track if we're already handling a 401 error
let isHandling401 = false

// Add response interceptor to handle token expiration
http.interceptors.response.use(
  (response) => response,
  (error) => {
    // Only handle 401 errors if we're not already handling one
    if (error.response?.status === 401 && !isHandling401) {
      isHandling401 = true

      // Clear token from localStorage only
      localStorage.removeItem('jwt_token')

      // Reset flag after a small delay to prevent multiple handlers
      setTimeout(() => {
        isHandling401 = false
      }, 1000)

      // Don't redirect here - let the component handle the error
    }
    return Promise.reject(error)
  }
)

export default http
