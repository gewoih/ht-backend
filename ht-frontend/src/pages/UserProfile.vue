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
            <div v-if="userStore.isSubscriptionExpired" class="subscription-expired-block">
              <i class="pi pi-lock expired-icon"></i>
              <h3>Подписка истекла</h3>
              <p>
                Доступ ко всем функциям временно ограничен. Пожалуйста, продлите подписку для
                продолжения использования сервиса.
              </p>
              <Button
                label="Продлить подписку"
                icon="pi pi-credit-card"
                class="p-button-lg"
                @click="showUpgradeDialog = true"
              />
            </div>
            <div v-else class="subscription-card">
              <div class="subscription-type paid">
                {{
                  userStore.user.subscription.endDate && userStore.isActiveSubscription
                    ? userStore.user.subscription.type === 1
                      ? 'Платная (Месяц/Год)'
                      : 'Платная'
                    : 'Нет активной подписки'
                }}
              </div>
              <div class="subscription-details">
                <div class="subscription-status">
                  <Badge
                    :value="userStore.isActiveSubscription ? 'Активна' : 'Неактивна'"
                    :severity="userStore.isActiveSubscription ? 'success' : 'danger'"
                  />
                </div>
                <div v-if="userStore.user.subscription.startDate" class="subscription-expires">
                  Начало: {{ formatDate(userStore.user.subscription.startDate) }}
                </div>
                <div v-if="userStore.user.subscription.endDate" class="subscription-expires">
                  Действует до: {{ formatDate(userStore.user.subscription.endDate) }}
                </div>
              </div>
              <div class="subscription-actions">
                <Button
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
      :header="userStore.isSubscriptionExpired ? 'Продлить подписку' : 'Управление подпиской'"
      :style="{
        width: '100%',
        maxWidth: '600px',
        background: '#f8fafc',
        borderRadius: '18px',
        boxShadow: '0 8px 32px rgba(0,0,0,0.10)',
      }"
      :modal="true"
    >
      <div class="upgrade-dialog-modern">
        <h3 class="upgrade-title">Выберите тариф</h3>
        <div class="modern-pricing-row">
          <div class="modern-pricing-card">
            <div class="modern-pricing-header">Месячный</div>
            <div class="modern-pricing-price">
              <span class="modern-price">349 ₽</span>
              <span class="modern-period">/ месяц</span>
            </div>
            <ul class="modern-benefits-list">
              <li><i class="pi pi-check"></i> Полный доступ ко всем функциям</li>
              <li><i class="pi pi-check"></i> Ежедневный дневник и аналитика</li>
              <li><i class="pi pi-check"></i> Приоритетная поддержка</li>
            </ul>
            <Button label="Выбрать" icon="pi pi-check" class="modern-choose-btn" />
          </div>
          <div class="modern-pricing-card highlighted">
            <div class="modern-popular-badge">2 месяца бесплатно</div>
            <div class="modern-pricing-header">Годовой</div>
            <div class="modern-pricing-price">
              <span class="modern-price">3490 ₽</span>
              <span class="modern-period">/ год</span>
            </div>
            <ul class="modern-benefits-list">
              <li><i class="pi pi-check"></i> Полный доступ ко всем функциям</li>
              <li><i class="pi pi-check"></i> Ежедневный дневник и аналитика</li>
              <li><i class="pi pi-check"></i> Приоритетная поддержка</li>
              <li><i class="pi pi-gift"></i> 2 месяца бесплатно</li>
            </ul>
            <Button label="Выбрать" icon="pi pi-check" class="modern-choose-btn highlighted" />
          </div>
        </div>
        <div v-if="userStore.isActiveSubscription" class="current-subscription-info-modern">
          <h4>Текущая подписка</h4>
          <p>Дата начала: {{ formatDate(userStore.user?.subscription.startDate) }}</p>
          <p>Действует до: {{ formatDate(userStore.user?.subscription.endDate) }}</p>
        </div>
      </div>
      <template #footer>
        <Button label="Закрыть" icon="pi pi-times" @click="showUpgradeDialog = false" text />
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
import Button from 'primevue/button'
import InputText from 'primevue/inputtext'
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

.upgrade-dialog-modern {
  background: #f8fafc;
  border-radius: 18px;
  padding: 2rem 1.5rem 1.5rem 1.5rem;
  box-shadow: 0 8px 32px rgba(0, 0, 0, 0.1);
}
.upgrade-title {
  text-align: center;
  font-size: 1.5rem;
  font-weight: 700;
  margin-bottom: 2rem;
  color: #1976d2;
}
.modern-pricing-row {
  display: flex;
  gap: 2rem;
  justify-content: center;
  align-items: stretch;
  margin-bottom: 2rem;
  flex-wrap: wrap;
}
.modern-pricing-card {
  background: #fff;
  border-radius: 14px;
  box-shadow: 0 2px 12px rgba(25, 118, 210, 0.06);
  padding: 2rem 1.5rem 1.5rem 1.5rem;
  min-width: 240px;
  max-width: 300px;
  flex: 1 1 240px;
  display: flex;
  flex-direction: column;
  align-items: center;
  position: relative;
  border: 2px solid transparent;
  transition: border 0.2s;
}
.modern-pricing-card.highlighted {
  border: 2px solid #1976d2;
  background: #f0f7ff;
}
.modern-popular-badge {
  position: absolute;
  top: -18px;
  left: 50%;
  transform: translateX(-50%);
  background: #1976d2;
  color: #fff;
  padding: 4px 18px;
  border-radius: 16px;
  font-size: 0.95rem;
  font-weight: 600;
  margin-bottom: 0.5rem;
  box-shadow: 0 2px 8px rgba(25, 118, 210, 0.1);
}
.modern-pricing-header {
  font-size: 1.2rem;
  font-weight: 700;
  color: #1976d2;
  margin-bottom: 0.5rem;
  text-align: center;
}
.modern-pricing-price {
  display: flex;
  align-items: baseline;
  justify-content: center;
  gap: 5px;
  margin-bottom: 1.2rem;
}
.modern-price {
  font-size: 2rem;
  font-weight: 800;
  color: #1976d2;
}
.modern-period {
  color: #6c757d;
  font-size: 1.1rem;
}
.modern-benefits-list {
  list-style: none;
  padding: 0;
  margin: 1.2rem 0 1.5rem 0;
  width: 100%;
}
.modern-benefits-list li {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  font-size: 1rem;
  color: #333;
  margin-bottom: 0.7rem;
}
.modern-benefits-list li i {
  color: #22c55e;
  font-size: 1.1rem;
}
.modern-choose-btn {
  width: 100%;
  font-weight: 700;
  font-size: 1.1rem;
  margin-top: auto;
  border-radius: 8px;
  background: #e3eefd;
  color: #1976d2;
  border: 1px solid #1976d2;
  transition: background 0.2s, color 0.2s;
}
.modern-choose-btn.highlighted {
  background: #1976d2;
  color: #fff;
  border: 1px solid #1976d2;
}
.modern-choose-btn:hover {
  background: #1976d2;
  color: #fff;
}
.current-subscription-info-modern {
  margin-top: 2rem;
  background: #f8f9fa;
  border-radius: 8px;
  padding: 1rem;
  text-align: left;
  box-shadow: 0 1px 4px rgba(25, 118, 210, 0.04);
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
