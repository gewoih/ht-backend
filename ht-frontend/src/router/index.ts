import { createRouter, createWebHistory } from 'vue-router'

const authRequiredRoutes = ['HabitJournal', 'Insights', 'Leaderboard', 'Profile', 'Analytics']

const router = createRouter({
  history: createWebHistory(),
  routes: [
    {
      path: '/login',
      name: 'Login',
      component: () => import('../pages/LoginPage.vue'),
    },
    {
      path: '/register',
      name: 'Register',
      component: () => import('../pages/RegisterPage.vue'),
    },
    {
      path: '/',
      name: 'Home',
      component: () => import('../pages/Home.vue'),
    },
    {
      path: '/journal',
      name: 'HabitJournal',
      component: () => import('../pages/HabitJournal.vue'),
      meta: { requiresAuth: true },
    },
    {
      path: '/analytics/insights',
      name: 'Insights',
      component: () => import('../pages/Insights.vue'),
      meta: { requiresAuth: true },
    },
    {
      path: '/leaderboard',
      name: 'Leaderboard',
      component: () => import('../pages/Leaderboard.vue'),
      meta: { requiresAuth: true },
    },
    {
      path: '/profile',
      name: 'Profile',
      component: () => import('../pages/UserProfile.vue'),
      meta: { requiresAuth: true },
    },
    {
      path: '/analytics/chart',
      name: 'Analytics',
      component: () => import('../pages/Analytics.vue'),
      meta: { requiresAuth: true },
    },
    {
      path: '/faq',
      name: 'FAQ',
      component: () => import('../pages/Faq.vue'),
    },
    {
      path: '/about',
      name: 'About',
      component: () => import('../pages/About.vue'),
    },
    {
      path: '/contact',
      name: 'Contact',
      component: () => import('../pages/Contact.vue'),
    },
  ],
})

router.beforeEach((to, from, next) => {
  const isAuthRequired = to.matched.some((record) => record.meta.requiresAuth)
  const accessToken = sessionStorage.getItem('access_token')

  if (isAuthRequired && !accessToken) {
    next({ name: 'Login' })
  } else {
    next()
  }
})

export default router
