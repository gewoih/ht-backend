<template>
  <div class="profile-container">
    <div class="profile-layout">
      <!-- Sidebar Menu -->
      <div class="profile-sidebar">
        <div class="menu-wrapper">
          <div
            v-for="item in menuItems"
            :key="item.label"
            class="menu-item"
            :class="{ active: activeSection === item.value }"
            @click="activeSection = item.value"
          >
            <i :class="item.icon"></i>
            <span>{{ item.label }}</span>
          </div>
        </div>
      </div>

      <!-- Content Area -->
      <div class="profile-content">
        <!-- Profile Section -->
        <div v-if="activeSection === 'profile'" class="content-section">
          <h2 class="section-title">Информация о пользователе</h2>

          <div v-if="userStore.isLoading" class="loading-container">
            <ProgressSpinner />
            <p>Загрузка профиля...</p>
          </div>

          <div v-else-if="userStore.hasError" class="error-container">
            <Message severity="error">{{ userStore.errorMessage }}</Message>
            <Button
              label="Повторить"
              icon="pi pi-refresh"
              @click="userStore.loadUserProfile"
              class="retry-button p-button-outlined"
            />
          </div>

          <div v-else-if="userStore.user" class="user-info-section">
            <div class="form-field">
              <label for="email">Email</label>
              <InputText
                id="email"
                type="email"
                v-model="userStore.user.email"
                disabled
                class="w-full"
              />
            </div>
            <!-- Additional profile fields can be added here -->
          </div>
        </div>

        <!-- Subscription Section -->
        <div v-if="activeSection === 'subscription'" class="content-section">
          <h2 class="section-title">Ваша подписка</h2>

          <div v-if="userStore.isLoading" class="loading-container">
            <ProgressSpinner />
            <p>Загрузка информации...</p>
          </div>

          <div v-else-if="userStore.hasError" class="error-container">
            <Message severity="error">{{ userStore.errorMessage }}</Message>
            <Button
              label="Повторить"
              icon="pi pi-refresh"
              @click="userStore.loadUserProfile"
              class="retry-button p-button-outlined"
            />
          </div>

          <div v-else-if="userStore.user" class="subscription-section">
            <div class="subscription-card">
              <div class="subscription-type" :class="{ premium: userStore.isPremium }">
                {{ userStore.isPremium ? 'PREMIUM' : 'БЕСПЛАТНАЯ' }}
              </div>

              <div class="subscription-details">
                <div class="subscription-status">
                  <Badge value="Активна" severity="success" />
                </div>

                <div v-if="userStore.user.subscription.endDate" class="subscription-expires">
                  Действует до: {{ formatDate(userStore.user.subscription.endDate) }}
                </div>
              </div>
            </div>

            <div class="subscription-limits">
              <h3>Лимиты</h3>
              <div class="limit-item">
                <div class="limit-label">Максимальное количество привычек</div>
                <div class="limit-value" :class="{ unlimited: userStore.isPremium }">
                  {{
                    userStore.isPremium ? 'Без ограничений' : userStore.subscriptionLimits.maxHabits
                  }}
                </div>
              </div>
            </div>

            <div class="subscription-actions">
              <Button
                v-if="!userStore.isPremium"
                label="Перейти на Premium"
                icon="pi pi-star"
                @click="showUpgradeDialog = true"
                severity="success"
                class="upgrade-button"
              />

              <Button
                v-else
                label="Управление подпиской"
                icon="pi pi-cog"
                @click="showUpgradeDialog = true"
                severity="secondary"
                class="manage-button p-button-outlined"
              />
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- Mobile menu button for responsive design -->
    <div class="mobile-menu-toggle">
      <Button icon="pi pi-bars" @click="showMobileMenu = !showMobileMenu" class="p-button-text" />
      <span class="menu-title">{{ getActiveMenuLabel() }}</span>
    </div>

    <!-- Mobile sidebar overlay -->
    <Dialog
      v-model:visible="showMobileMenu"
      position="left"
      :modal="true"
      :showHeader="false"
      :dismissableMask="true"
      :closeOnEscape="true"
      :style="{ width: '80%', maxWidth: '250px' }"
      class="mobile-sidebar-dialog"
    >
      <div class="mobile-menu-wrapper">
        <div
          v-for="item in menuItems"
          :key="item.label"
          class="menu-item"
          :class="{ active: activeSection === item.value }"
          @click="
            () => {
              activeSection = item.value
              showMobileMenu = false
            }
          "
        >
          <i :class="item.icon"></i>
          <span>{{ item.label }}</span>
        </div>
      </div>
    </Dialog>

    <Dialog
      v-model:visible="showUpgradeDialog"
      :header="userStore.isPremium ? 'Управление подпиской' : 'Переход на Premium'"
      :style="{ width: '90%', maxWidth: '450px' }"
      :modal="true"
    >
      <div class="upgrade-dialog-content">
        <template v-if="!userStore.isPremium">
          <h3>Преимущества Premium-подписки:</h3>
          <ul class="benefits-list">
            <li><i class="pi pi-check"></i> Без ограничений на количество привычек</li>
            <li><i class="pi pi-check"></i> Расширенная аналитика и отчеты</li>
            <li><i class="pi pi-check"></i> Приоритетная поддержка</li>
          </ul>
          <div class="pricing">
            <div class="price">499 руб/месяц</div>
            <div class="billing-note">Оплата раз в месяц, можно отменить в любое время</div>
          </div>
        </template>
        <template v-else>
          <h3>Информация о вашей подписке</h3>
          <p>У вас активна Premium-подписка.</p>
          <p>Дата начала: {{ formatDate(userStore.user?.subscription.startDate) }}</p>
          <p>Действует до: {{ formatDate(userStore.user?.subscription.endDate) }}</p>
        </template>
      </div>

      <template #footer>
        <Button label="Закрыть" icon="pi pi-times" @click="showUpgradeDialog = false" text />
        <Button
          v-if="!userStore.isPremium"
          label="Оформить подписку"
          icon="pi pi-check"
          severity="success"
          autofocus
        />
      </template>
    </Dialog>

    <Toast position="bottom-right" />
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useUserStore } from '../stores/userStore'
import { format } from 'date-fns'
import { ru } from 'date-fns/locale'
import Card from 'primevue/card'
import Button from 'primevue/button'
import InputText from 'primevue/inputtext'
import Menu from 'primevue/menu'
import ProgressSpinner from 'primevue/progressspinner'
import Message from 'primevue/message'
import Dialog from 'primevue/dialog'
import Badge from 'primevue/badge'
import Toast from 'primevue/toast'

const userStore = useUserStore()
const showUpgradeDialog = ref(false)
const showMobileMenu = ref(false)
const activeSection = ref('profile')

const menuItems = [
  {
    label: 'Профиль',
    icon: 'pi pi-user',
    value: 'profile',
  },
  {
    label: 'Подписка',
    icon: 'pi pi-star',
    value: 'subscription',
  },
]

function getActiveMenuLabel() {
  const activeItem = menuItems.find((item) => item.value === activeSection.value)
  return activeItem ? activeItem.label : ''
}

onMounted(async () => {
  if (!userStore.user) {
    await userStore.loadUserProfile()
  }
})

function formatDate(dateString?: string) {
  if (!dateString) return 'Н/Д'

  // Handle special case for max date
  if (dateString.includes('9999-12-31')) {
    return 'Бессрочно'
  }

  return format(new Date(dateString), 'dd MMMM yyyy', { locale: ru })
}
</script>

<style>
.profile-container {
  max-width: 1000px;
  margin: 2rem auto;
  padding: 0 1rem;
  position: relative;
}

.profile-layout {
  display: flex;
  gap: 2rem;
}

.profile-sidebar {
  width: 220px;
  flex-shrink: 0;
}

.menu-wrapper,
.mobile-menu-wrapper {
  background-color: #ffffff;
  border-radius: 12px;
  overflow: hidden;
  box-shadow: 0 2px 12px rgba(0, 0, 0, 0.04);
  border: 1px solid #f0f0f0;
}

.menu-item {
  display: flex;
  align-items: center;
  padding: 0.85rem 1.25rem;
  cursor: pointer;
  transition: all 0.2s ease;
  border-left: 3px solid transparent;
}

.menu-item:hover {
  background-color: rgba(25, 118, 210, 0.04);
}

.menu-item.active {
  background-color: rgba(25, 118, 210, 0.08);
  border-left-color: var(--primary-color);
}

.menu-item i {
  margin-right: 0.75rem;
  font-size: 1rem;
  color: #666;
}

.menu-item.active i {
  color: var(--primary-color);
}

.menu-item span {
  font-weight: 500;
  color: #444;
}

.menu-item.active span {
  color: var(--primary-color);
}

.profile-content {
  flex: 1;
}

.content-section {
  background: #ffffff;
  border-radius: 12px;
  box-shadow: 0 2px 12px rgba(0, 0, 0, 0.04);
  padding: 1.5rem;
  border: 1px solid #f0f0f0;
}

.section-title {
  font-size: 1.4rem;
  font-weight: 600;
  color: #333;
  margin-bottom: 1.5rem;
  padding-bottom: 1rem;
  border-bottom: 1px solid #f0f0f0;
}

.mobile-menu-toggle {
  display: none;
  align-items: center;
  padding: 0.75rem 0;
  margin-bottom: 1rem;
}

.menu-title {
  font-size: 1.2rem;
  font-weight: 600;
  margin-left: 0.5rem;
  color: #333;
}

.mobile-sidebar-dialog .p-dialog-content {
  padding: 0;
}

.user-info-section,
.subscription-section {
  padding: 0.5rem 0;
}

.form-field {
  margin-bottom: 1.5rem;
}

.form-field label {
  display: block;
  margin-bottom: 0.5rem;
  font-weight: 500;
  color: #555;
}

.subscription-card {
  background-color: #f8f9fa;
  border-radius: 10px;
  padding: 1.25rem;
  margin-bottom: 1.5rem;
  border: 1px solid #eee;
  transition: box-shadow 0.2s ease;
}

.subscription-card:hover {
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.05);
}

.subscription-type {
  font-size: 1.25rem;
  font-weight: 700;
  margin-bottom: 0.5rem;
  color: #555;
}

.subscription-type.premium {
  color: #f59e0b;
}

.subscription-details {
  display: flex;
  justify-content: space-between;
  align-items: center;
  font-size: 0.9rem;
  color: #6c757d;
  margin-top: 0.75rem;
}

.subscription-limits {
  margin: 2rem 0;
}

.subscription-limits h3 {
  font-size: 1.1rem;
  font-weight: 600;
  margin-bottom: 1rem;
  color: #444;
}

.limit-item {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 0.75rem 1rem;
  background-color: #f8f9fa;
  border-radius: 8px;
  margin-bottom: 0.75rem;
}

.limit-label {
  color: #555;
  font-weight: 500;
}

.limit-value {
  font-weight: 600;
  color: #444;
}

.limit-value.unlimited {
  color: #10b981;
}

.subscription-actions {
  margin-top: 2rem;
}

.loading-container,
.error-container {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  padding: 3rem 2rem;
  text-align: center;
}

.retry-button {
  margin-top: 1rem;
}

.upgrade-dialog-content {
  text-align: center;
  padding: 1rem 0;
}

.benefits-list {
  text-align: left;
  margin: 1.5rem 0;
  padding-left: 1rem;
}

.benefits-list li {
  margin-bottom: 0.75rem;
  display: flex;
  align-items: center;
}

.benefits-list li i {
  color: #10b981;
  margin-right: 0.75rem;
}

.pricing {
  margin-top: 2rem;
  padding: 1rem;
  background-color: #f0f7ff;
  border-radius: 8px;
}

.price {
  font-size: 1.75rem;
  font-weight: 700;
  color: var(--primary-color);
}

.billing-note {
  font-size: 0.85rem;
  color: #6c757d;
  margin-top: 0.5rem;
}

/* Responsive adjustments */
@media (max-width: 768px) {
  .profile-container {
    padding: 0 0.75rem;
    margin: 1rem auto;
  }

  .profile-layout {
    flex-direction: column;
  }

  .profile-sidebar {
    display: none;
  }

  .mobile-menu-toggle {
    display: flex;
    background-color: #ffffff;
    border-radius: 8px;
    padding: 0.75rem 1rem;
    box-shadow: 0 2px 12px rgba(0, 0, 0, 0.04);
    border: 1px solid #f0f0f0;
  }

  .content-section {
    padding: 1.25rem 1rem;
  }

  .section-title {
    font-size: 1.2rem;
    margin-bottom: 1.25rem;
  }

  .limit-item {
    flex-direction: column;
    align-items: flex-start;
  }

  .limit-label {
    margin-bottom: 0.5rem;
  }

  .upgrade-button,
  .manage-button {
    width: 100%;
  }
}
</style>
