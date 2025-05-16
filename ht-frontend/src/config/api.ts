export const API_ENDPOINTS = {
  habits: `/habits`,
  userHabits: `/me/habits`,
  journal: `/me/journal`,
  insights: `/me/insight`,
  auth: {
    login: `/auth/login`,
    register: `/auth/register`,
    refresh: `/auth/refresh`,
    logout: `/auth/logout`,
    confirmEmail: `/auth/confirm-email`,
    emailConfirmation: `/auth/email-confirmation`,
  },
  user: {
    profile: `/me/profile`,
    analytics: `/me/analytics`,
  },
  leaderboard: `/leaderboard`,
}
