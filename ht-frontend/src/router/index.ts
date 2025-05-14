import { createRouter, createWebHistory } from 'vue-router'

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
    },
    {
      path: '/analytics/insights',
      name: 'Insights',
      component: () => import('../pages/Insights.vue'),
    },
    {
      path: '/leaderboard',
      name: 'Leaderboard',
      component: () => import('../pages/Leaderboard.vue'),
    },
    {
      path: '/profile',
      name: 'Profile',
      component: () => import('../pages/UserProfile.vue'),
    },
    {
      path: '/analytics/chart',
      name: 'Analytics',
      component: () => import('../pages/Analytics.vue'),
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

export default router
