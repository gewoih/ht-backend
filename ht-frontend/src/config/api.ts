export const API_BASE_URL = 'https://localhost:5001/api'

export const API_ENDPOINTS = {
  habits: `${API_BASE_URL}/habits`,
  userHabits: `${API_BASE_URL}/me/habits`,
  journal: `${API_BASE_URL}/me/journal`,
  insights: `${API_BASE_URL}/me/insight`,
  auth: {
    login: `${API_BASE_URL}/auth/login`,
    register: `${API_BASE_URL}/auth/register`,
  },
  user: {
    profile: `${API_BASE_URL}/me/profile`,
    analytics: `${API_BASE_URL}/me/analytics`,
  },
  leaderboard: `${API_BASE_URL}/leaderboard`,
}
