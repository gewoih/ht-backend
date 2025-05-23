<template>
  <header class="app-header">
    <div class="header-content">
      <div class="logo-container" @click="router.push('/')" style="cursor: pointer">
        <img src="../../assets/logo.svg" alt="Habit Tracker Logo" class="logo" />
        <h1 class="app-title">TrackMe</h1>
      </div>

      <template v-if="isLoggedIn">
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
          <Button class="profile-btn" :text="true" @click="toggleProfileMenu">
            <div class="profile-content">
              <Avatar
                :label="getUserInitial(userName)"
                shape="circle"
                class="avatar"
                :style="{ backgroundColor: generateAvatarColor(userName) }"
              />
              <span class="username">{{ userName || 'User' }}</span>
              <i class="pi pi-chevron-down"></i>
            </div>
          </Button>
          <div v-if="showProfileMenu" class="profile-dropdown">
            <div class="profile-info">
              <Avatar
                :label="getUserInitial(userName)"
                shape="circle"
                class="dropdown-avatar"
                :style="{ backgroundColor: generateAvatarColor(userName) }"
              />
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
        </div>
      </template>
      <template v-else>
        <div class="user-actions">
          <Button label="Войти в аккаунт" text @click="router.push('/login')" />
        </div>
      </template>

      <!-- Mobile Menu Toggle -->
      <div class="mobile-menu-toggle" v-if="isLoggedIn">
        <Button icon="pi pi-bars" text @click="toggleMobileMenu" />
      </div>
    </div>

    <!-- Mobile Menu -->
    <div class="mobile-menu" :class="{ 'mobile-menu-open': showMobileMenu }" v-if="isLoggedIn">
      <div class="mobile-menu-header">
        <div class="logo-container">
          <img src="../../assets/logo.svg" alt="Habit Tracker Logo" class="logo" />
          <h1 class="app-title">TrackMe</h1>
        </div>
        <Button icon="pi pi-times" text class="close-menu-btn" @click="showMobileMenu = false" />
      </div>

      <div class="mobile-menu-content">
        <div
          v-for="menuItem in menuItems.filter((item) => !item.hasSubmenu)"
          :key="menuItem.path"
          class="mobile-menu-item"
          :class="{ 'active-mobile-item': isActive(menuItem.path) }"
          @click="navigateToMobile(menuItem.path)"
        >
          <span>{{ menuItem.label }}</span>
        </div>

        <!-- Analytics mobile dropdown -->
        <div class="mobile-submenu-container">
          <div
            class="mobile-menu-item"
            :class="{ 'active-mobile-item': isActive('/analytics') }"
            @click="toggleMobileSubmenu"
          >
            <span>Аналитика</span>
            <i class="pi pi-chevron-down" :class="{ 'dropdown-active': showMobileSubmenu }"></i>
          </div>

          <div class="mobile-submenu" v-if="showMobileSubmenu">
            <div
              class="mobile-submenu-item"
              :class="{ 'active-mobile-item': isActive('/analytics/insights') }"
              @click="navigateToMobile('/analytics/insights')"
            >
              <i class="pi pi-chart-bar"></i>
              <span>Влияние привычек</span>
            </div>
            <div
              class="mobile-submenu-item"
              :class="{ 'active-mobile-item': isActive('/analytics/chart') }"
              @click="navigateToMobile('/analytics/chart')"
            >
              <i class="pi pi-chart-line"></i>
              <span>График</span>
            </div>
          </div>
        </div>
      </div>

      <div class="mobile-menu-footer">
        <div class="mobile-user-info">
          <Avatar
            :label="getUserInitial(userName)"
            shape="circle"
            class="mobile-avatar"
            :style="{ backgroundColor: generateAvatarColor(userName) }"
          />
          <div class="mobile-user-details">
            <h3>{{ userName || 'User' }}</h3>
            <p>{{ userEmail }}</p>
          </div>
        </div>
        <div class="mobile-user-actions">
          <Button
            v-for="item in profileMenuItems"
            :key="item.label"
            :label="item.label"
            :icon="item.icon"
            text
            class="mobile-action-btn"
            @click="handleMobileAction(item.action)"
          />
        </div>
      </div>
    </div>
  </header>
</template>

<script setup lang="ts">
import { ref, onMounted, computed, onBeforeUnmount, watch } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import Button from 'primevue/button'
import Avatar from 'primevue/avatar'
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

// Mobile menu state
const showMobileMenu = ref(false)
const showMobileSubmenu = ref(false)

// Toggle analytics menu
const toggleAnalyticsMenu = () => {
  showAnalyticsMenu.value = !showAnalyticsMenu.value
}

const hideAnalyticsMenu = () => {
  showAnalyticsMenu.value = false
}

// Toggle mobile menu
const toggleMobileMenu = () => {
  showMobileMenu.value = !showMobileMenu.value
  showMobileSubmenu.value = false
}

// Toggle mobile submenu
const toggleMobileSubmenu = () => {
  showMobileSubmenu.value = !showMobileSubmenu.value
}

// User data from JWT token
const userName = ref('')
const userEmail = ref('')

function parseJwt(token: string) {
  try {
    const base64Url = token.split('.')[1]
    const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/')
    const jsonPayload = decodeURIComponent(
      atob(base64)
        .split('')
        .map((c) => '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2))
        .join('')
    )
    return JSON.parse(jsonPayload)
  } catch (error) {
    console.error('Error parsing JWT', error)
    return {}
  }
}

function loadUserDataFromToken() {
  if (isAuthenticated()) {
    const token = auth.jwtToken
    if (token) {
      const decoded = parseJwt(token)
      userName.value = decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'] || ''
      userEmail.value =
        decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'] || ''
    }
  } else {
    userName.value = ''
    userEmail.value = ''
  }
}

// Generate color based on username
function generateAvatarColor(username: string) {
  if (!username) return '#4caf50'

  // Simple hash function for consistent color
  let hash = 0
  for (let i = 0; i < username.length; i++) {
    hash = username.charCodeAt(i) + ((hash << 5) - hash)
  }

  // Convert to hex color with good saturation and lightness (avoid too dark/light)
  const hue = Math.abs(hash % 360)
  return `hsl(${hue}, 65%, 55%)`
}

// Get user initial for avatar
function getUserInitial(username: string): string {
  return username && username.length > 0 ? username[0].toUpperCase() : ''
}

// Profile menu
const showProfileMenu = ref(false)
const profileMenuItems = ref([
  { label: 'Мой профиль', icon: 'pi pi-user', action: 'profile' },
  { label: 'Выйти', icon: 'pi pi-sign-out', action: 'logout' },
])

onMounted(() => {
  // Check authentication status and update local state if needed
  if (isAuthenticated() && !auth.isLoggedIn) {
    // This shouldn't happen, but just in case
    auth.setToken('', false)
  } else if (!isAuthenticated() && auth.isLoggedIn) {
    auth.clearToken()
  }

  // Load user data from token
  loadUserDataFromToken()

  // Add event listener for auth changes
  window.addEventListener('auth:login-success', handleAuthChange)

  // Close analytics menu when clicking outside
  document.addEventListener('click', (event: MouseEvent) => {
    if (
      analyticsMenuContainer.value &&
      !analyticsMenuContainer.value.contains(event.target as Node)
    ) {
      showAnalyticsMenu.value = false
    }

    // Close profile menu when clicking outside
    const target = event.target as HTMLElement
    if (
      showProfileMenu.value &&
      !target.closest('.profile-btn') &&
      !target.closest('.profile-dropdown')
    ) {
      showProfileMenu.value = false
    }
  })

  // Close mobile menu on window resize
  window.addEventListener('resize', () => {
    if (window.innerWidth > 768 && showMobileMenu.value) {
      showMobileMenu.value = false
    }
  })
})

onBeforeUnmount(() => {
  // Remove event listeners when component is unmounted
  window.removeEventListener('auth:login-success', handleAuthChange)
  document.removeEventListener('click', () => {})
  window.removeEventListener('resize', () => {})
})

const handleAuthChange = () => {
  // Force the component to recognize the auth state has changed
  if (isAuthenticated()) {
    loadUserDataFromToken()
  }
}

// Watch for auth state changes to reload user data
watch(
  () => auth.isLoggedIn,
  (newVal) => {
    if (newVal) {
      loadUserDataFromToken()
    } else {
      userName.value = ''
      userEmail.value = ''
    }
  }
)

const isActive = (path: string) => {
  return route.path === path
}

const navigateTo = (path: string) => {
  router.push(path)
  hideAnalyticsMenu()
}

const navigateToMobile = (path: string) => {
  router.push(path)
  showMobileMenu.value = false
  showMobileSubmenu.value = false
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
      showProfileMenu.value = false
      logout()
      break
  }
  showProfileMenu.value = false
}

const handleMobileAction = (action: string) => {
  handleProfileAction(action)
  showMobileMenu.value = false
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

/* Mobile menu toggle */
.mobile-menu-toggle {
  display: none;
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
  background: white;
  border-radius: 8px;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
  min-width: 300px;
  max-height: 400px;
  overflow-y: auto;
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

/* Mobile Menu */
.mobile-menu {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: white;
  z-index: 2000;
  transform: translateX(100%);
  transition: transform 0.3s ease;
  display: flex;
  flex-direction: column;
  overflow-y: auto;
}

.mobile-menu-open {
  transform: translateX(0);
}

.mobile-menu-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 1rem;
  border-bottom: 1px solid #eee;
}

.close-menu-btn .p-button-icon {
  font-size: 1.5rem;
}

.mobile-menu-content {
  flex: 1;
  padding: 1rem 0;
  display: flex;
  flex-direction: column;
}

.mobile-menu-item {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 1rem 1.5rem;
  font-size: 1.1rem;
  font-weight: 500;
  color: #2c3e50;
  border-bottom: 1px solid #f5f5f5;
  cursor: pointer;
}

.mobile-menu-item:active {
  background-color: #f8f9fa;
}

.active-mobile-item {
  color: #4caf50;
  background-color: rgba(76, 175, 80, 0.08);
}

.mobile-submenu-container {
  display: flex;
  flex-direction: column;
}

.mobile-submenu {
  background-color: #f8f9fa;
  border-bottom: 1px solid #f0f0f0;
}

.mobile-submenu-item {
  display: flex;
  align-items: center;
  gap: 0.75rem;
  padding: 1rem 2rem;
  font-size: 1rem;
  color: #2c3e50;
  cursor: pointer;
}

.mobile-submenu-item i {
  color: #4caf50;
  font-size: 1.1rem;
}

.mobile-menu-footer {
  padding: 1rem;
  border-top: 1px solid #eee;
}

.mobile-user-info {
  display: flex;
  align-items: center;
  gap: 1rem;
  margin-bottom: 1rem;
}

.mobile-avatar {
  width: 48px;
  height: 48px;
  border-radius: 50%;
  object-fit: cover;
}

.mobile-user-details h3 {
  margin: 0;
  font-size: 1.1rem;
  color: #2c3e50;
}

.mobile-user-details p {
  margin: 0;
  font-size: 0.9rem;
  color: #666;
}

.mobile-user-actions {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
}

.mobile-action-btn {
  width: 100%;
  justify-content: flex-start;
  padding: 0.8rem 1rem;
  margin-bottom: 0.25rem;
}

.mobile-auth-buttons {
  display: flex;
  flex-direction: column;
  gap: 0.75rem;
}

.mobile-register-btn,
.mobile-login-btn {
  width: 100%;
  padding: 0.8rem 1rem;
  justify-content: center;
}

/* Responsive Design */
@media (max-width: 768px) {
  .menu-container,
  .user-actions {
    display: none;
  }

  .header-content {
    padding: 0.75rem 1rem;
    gap: 0;
  }

  .mobile-menu-toggle {
    display: block;
  }

  .mobile-menu-toggle .p-button {
    font-size: 1.3rem;
  }

  .logo {
    height: 32px;
  }

  .app-title {
    font-size: 1.3rem;
  }

  .profile-dropdown {
    right: 0.5rem;
    width: calc(100% - 1rem);
    max-width: 300px;
  }
}

@media (max-width: 480px) {
  .header-content {
    padding: 0.75rem 0.75rem;
  }

  .app-title {
    font-size: 1.2rem;
  }

  .mobile-menu-item,
  .mobile-submenu-item {
    padding-top: 1.1rem;
    padding-bottom: 1.1rem;
  }

  .mobile-menu-footer {
    padding: 1rem 1.5rem 2rem;
  }
}
</style>
