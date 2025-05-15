<template>
  <header class="app-header">
    <div class="header-content">
      <div class="logo-container" @click="router.push('/')" style="cursor: pointer">
        <img src="../../assets/logo.svg" alt="Habit Tracker Logo" class="logo" />
        <h1 class="app-title">TrackMe</h1>
      </div>

      <div class="menu-container">
        <Button
          v-for="menuItem in menuItems.filter((item) => !item.hasSubmenu)"
          :key="menuItem.path"
          :label="menuItem.label"
          :text="true"
          :class="{ 'active-menu': isActive(menuItem.path) }"
          @click="navigateTo(menuItem.path)"
        />

        <!-- Analytics dropdown menu -->
        <div class="analytics-menu-container" ref="analyticsMenuContainer">
          <Button
            ref="analyticsBtnRef"
            :class="{ 'active-menu': isActive('/analytics') || isActive('/insights') }"
            @click="toggleAnalyticsMenu"
            text
            aria-haspopup="true"
            aria-controls="analytics_menu"
            class="analytics-button"
          >
            <span>Аналитика</span>
            <i
              class="pi pi-chevron-down analytics-dropdown-icon"
              :class="{ 'dropdown-active': showAnalyticsMenu }"
            ></i>
          </Button>

          <div
            class="custom-menu-overlay"
            v-if="showAnalyticsMenu"
            @click="hideAnalyticsMenu"
          ></div>

          <div class="custom-analytics-menu" v-show="showAnalyticsMenu" @click.stop>
            <div
              class="menu-item"
              @click="navigateTo('/analytics/insights')"
              :class="{ 'active-item': isActive('/analytics/insights') }"
            >
              <i class="pi pi-chart-bar"></i>
              <span>Влияние привычек</span>
            </div>
            <div
              class="menu-item"
              @click="navigateTo('/analytics/chart')"
              :class="{ 'active-item': isActive('/analytics/chart') }"
            >
              <i class="pi pi-chart-line"></i>
              <span>График</span>
            </div>
          </div>
        </div>
      </div>

      <div class="user-actions">
        <template v-if="isLoggedIn">
          <Button class="profile-btn" :text="true" @click="toggleProfileMenu">
            <div class="profile-content">
              <img :src="userAvatar" alt="User Avatar" class="avatar" />
              <span class="username">{{ userName || 'User' }}</span>
              <i class="pi pi-chevron-down"></i>
            </div>
          </Button>
        </template>
        <template v-else>
          <Button
            class="register-btn"
            label="Register"
            icon="pi pi-user-plus"
            text
            @click="router.push('/register')"
          />
          <Button
            class="login-btn"
            label="Login"
            icon="pi pi-sign-in"
            outlined
            @click="router.push('/login')"
          />
        </template>
      </div>
    </div>

    <!-- Profile Dropdown -->
    <div v-if="showProfileMenu" class="profile-dropdown">
      <div class="profile-info">
        <img :src="userAvatar" alt="User Avatar" class="dropdown-avatar" />
        <div class="user-details">
          <h3>{{ userName || 'User' }}</h3>
          <p>{{ userEmail }}</p>
        </div>
      </div>
      <div class="dropdown-menu">
        <Button
          v-for="item in profileMenuItems"
          :key="item.label"
          :label="item.label"
          :icon="item.icon"
          text
          class="dropdown-item"
          @click="handleProfileAction(item.action)"
        />
      </div>
    </div>
  </header>
</template>

<script setup lang="ts">
import { ref, onMounted, computed, onBeforeUnmount } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import Button from 'primevue/button'
import { logout, isAuthenticated } from '../../services/auth.service'
import { useAuthStore } from '../../stores/authStore'

const auth = useAuthStore()
const isLoggedIn = computed(() => auth.isLoggedIn)
const router = useRouter()
const route = useRoute()

// Menu items
const menuItems = ref([
  { label: 'Журнал', path: '/journal', hasSubmenu: false },
  { label: 'Аналитика', path: '/analytics', hasSubmenu: true },
  { label: 'Зал славы', path: '/leaderboard', hasSubmenu: false },
])

// Analytics menu
const analyticsBtnRef = ref()
const analyticsMenuContainer = ref()
const showAnalyticsMenu = ref(false)

// Toggle analytics menu
const toggleAnalyticsMenu = () => {
  showAnalyticsMenu.value = !showAnalyticsMenu.value
}

const hideAnalyticsMenu = () => {
  showAnalyticsMenu.value = false
}

// User data (mock for now)
const userName = ref('')
const userEmail = ref('')
const userAvatar = ref('https://i.pravatar.cc/150?img=3')

// Profile menu
const showProfileMenu = ref(false)
const profileMenuItems = ref([
  { label: 'Мой профиль', icon: 'pi pi-user', action: 'profile' },
  { label: 'Настройки', icon: 'pi pi-cog', action: 'settings' },
  { label: 'Выйти', icon: 'pi pi-sign-out', action: 'logout' },
])

onMounted(() => {
  // Check authentication status and update local state if needed
  if (isAuthenticated() && !auth.isLoggedIn) {
    auth.setToken(sessionStorage.getItem('access_token') || '')
  } else if (!isAuthenticated() && auth.isLoggedIn) {
    auth.clearToken()
  }

  // Add event listener for auth changes
  window.addEventListener('auth:login-success', handleAuthChange)

  // Close analytics menu when clicking outside
  document.addEventListener('click', (event) => {
    if (analyticsMenuContainer.value && !analyticsMenuContainer.value.contains(event.target)) {
      showAnalyticsMenu.value = false
    }
  })
})

onBeforeUnmount(() => {
  // Remove event listeners when component is unmounted
  window.removeEventListener('auth:login-success', handleAuthChange)
  document.removeEventListener('click', () => {})
})

const handleAuthChange = () => {
  // Force the component to recognize the auth state has changed
  if (isAuthenticated()) {
    auth.setToken(sessionStorage.getItem('access_token') || '')
  }
}

const isActive = (path: string) => {
  return route.path === path
}

const navigateTo = (path: string) => {
  router.push(path)
  hideAnalyticsMenu()
}

const toggleProfileMenu = () => {
  showProfileMenu.value = !showProfileMenu.value
}

const handleProfileAction = (action: string) => {
  switch (action) {
    case 'profile':
      router.push('/profile')
      break
    case 'settings':
      router.push('/settings')
      break
    case 'logout':
      logout()
      break
  }
  showProfileMenu.value = false
}
</script>

<style lang="css" scoped>
.app-header {
  background: #fff;
  width: 100%;
  position: relative;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.05);
  z-index: 1000;
}

.header-content {
  max-width: 1200px;
  width: 100%;
  margin: 0 auto;
  padding: 1rem;
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: 2rem;
}

/* Logo and Title */
.logo-container {
  display: flex;
  align-items: center;
  gap: 1rem;
}

.logo {
  height: 40px;
  width: auto;
}

.app-title {
  font-size: 1.5rem;
  font-weight: 600;
  color: #2c3e50;
  margin: 0;
}

/* Navigation Menu */
.menu-container {
  display: flex;
  gap: 1rem;
  flex: 1;
  justify-content: center;
  align-items: center;
}

.menu-container .p-button {
  font-weight: 500;
  color: #666;
  padding: 0.5rem 1rem;
  transition: all 0.3s ease;
}

.menu-container .p-button.active-menu {
  color: #4caf50;
  font-weight: 600;
  background: rgba(76, 175, 80, 0.1);
  border-radius: 8px;
}

.menu-container .p-button:hover {
  color: #4caf50;
  background: rgba(76, 175, 80, 0.1);
}

/* Analytics dropdown */
.analytics-menu-container {
  position: relative;
  display: inline-flex;
}

.analytics-button {
  display: flex;
  align-items: center;
  gap: 0.4rem;
  font-weight: 500;
  border-radius: 8px;
}

.analytics-dropdown-icon {
  font-size: 0.7rem;
  transition: transform 0.3s ease;
}

.dropdown-active {
  transform: rotate(180deg);
}

.custom-menu-overlay {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  z-index: 999;
}

.custom-analytics-menu {
  position: absolute;
  top: calc(100% + 8px);
  left: 50%;
  transform: translateX(-50%);
  background: white;
  box-shadow: 0 10px 25px rgba(0, 0, 0, 0.1), 0 6px 12px rgba(0, 0, 0, 0.05);
  border-radius: 12px;
  overflow: hidden;
  width: 210px;
  z-index: 1000;
  animation: fadeIn 0.2s ease;
}

@keyframes fadeIn {
  from {
    opacity: 0;
    transform: translateX(-50%) translateY(-5px);
  }
  to {
    opacity: 1;
    transform: translateX(-50%) translateY(0);
  }
}

.menu-item {
  padding: 0.85rem 1.2rem;
  display: flex;
  align-items: center;
  gap: 0.75rem;
  transition: all 0.2s ease;
  cursor: pointer;
  font-weight: 500;
  color: #444;
}

.menu-item:hover {
  background-color: rgba(76, 175, 80, 0.08);
  color: #4caf50;
}

.menu-item.active-item {
  background-color: rgba(76, 175, 80, 0.1);
  color: #4caf50;
  font-weight: 600;
}

.menu-item i {
  font-size: 1rem;
  color: #4caf50;
}

/* User Actions */
.user-actions {
  display: flex;
  align-items: center;
  gap: 1rem;
}

.profile-btn {
  padding: 0.5rem;
}

.profile-content {
  display: flex;
  align-items: center;
  gap: 0.75rem;
}

.avatar {
  width: 32px;
  height: 32px;
  border-radius: 50%;
  object-fit: cover;
}

.username {
  font-size: 0.9rem;
  color: #2c3e50;
}

.profile-dropdown {
  position: absolute;
  top: 100%;
  right: 0;
  background: white;
  border-radius: 8px;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
  min-width: 300px;
  max-height: 400px;
  overflow-y: auto;
}

.profile-dropdown {
  right: 1rem;
  width: 250px;
}

/* Profile Dropdown */
.profile-info {
  padding: 1rem;
  border-bottom: 1px solid #eee;
  display: flex;
  gap: 1rem;
  align-items: center;
}

.dropdown-avatar {
  width: 48px;
  height: 48px;
  border-radius: 50%;
  object-fit: cover;
}

.user-details h3 {
  margin: 0;
  font-size: 1rem;
  color: #2c3e50;
}

.user-details p {
  margin: 0;
  font-size: 0.8rem;
  color: #666;
}

.dropdown-menu {
  padding: 0.5rem;
}

.dropdown-item {
  width: 100%;
  justify-content: flex-start;
  padding: 0.75rem 1rem;
  color: #2c3e50;
  gap: 0.75rem;
}

.dropdown-item:hover {
  background-color: #f8f9fa;
  color: #4caf50;
}

/* Responsive Design */
@media (max-width: 768px) {
  .header-content {
    flex-direction: column;
    gap: 1rem;
  }

  .menu-container {
    order: 3;
    width: 100%;
    overflow-x: auto;
    padding: 0.5rem 0;
  }

  .user-actions {
    order: 2;
    width: 100%;
    justify-content: flex-end;
  }

  .logo-container {
    order: 1;
    width: 100%;
    justify-content: center;
  }

  .profile-dropdown {
    position: fixed;
    top: 0;
    right: 0;
    width: 100%;
    max-width: 100%;
    height: 100vh;
    border-radius: 0;
  }

  .custom-analytics-menu {
    position: fixed;
    top: auto;
    bottom: 0;
    left: 0;
    right: 0;
    width: 100%;
    transform: none;
    border-radius: 16px 16px 0 0;
    animation: slideUp 0.3s ease;
  }

  @keyframes slideUp {
    from {
      transform: translateY(100%);
    }
    to {
      transform: translateY(0);
    }
  }

  .menu-item {
    padding: 1rem 1.5rem;
  }
}
</style>
