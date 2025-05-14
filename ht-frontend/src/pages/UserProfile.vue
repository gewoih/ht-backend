<template>
  <div class="profile-container">
    <Card class="profile-card">
      <template #title>
        <div class="profile-header">
          <span class="profile-title">Ваш профиль</span>
        </div>
      </template>

      <template #content>
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
            class="retry-button"
          />
        </div>

        <div v-else-if="userStore.user" class="profile-content">
          <div class="user-info-section">
            <h3>Информация о пользователе</h3>
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
          </div>

          <div class="subscription-section">
            <h3>Ваша подписка</h3>
            <div class="subscription-info">
              <div class="subscription-type" :class="{ premium: userStore.isPremium }">
                {{ userStore.isPremium ? 'PREMIUM' : 'БЕСПЛАТНАЯ' }}
              </div>

              <div class="subscription-details">
                <div class="subscription-status">
                  Статус: <Badge value="Активна" severity="success" />
                </div>

                <div v-if="userStore.user.subscription.endDate" class="subscription-expires">
                  Действует до: {{ formatDate(userStore.user.subscription.endDate) }}
                </div>
              </div>
            </div>

            <div class="subscription-limits">
              <h4>Лимиты</h4>
              <div class="limit-item">
                <div class="limit-label">Максимальное количество привычек:</div>
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
                class="manage-button"
              />
            </div>
          </div>
        </div>
      </template>
    </Card>

    <Dialog
      v-model:visible="showUpgradeDialog"
      :header="userStore.isPremium ? 'Управление подпиской' : 'Переход на Premium'"
      :style="{ width: '450px' }"
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
import ProgressSpinner from 'primevue/progressspinner'
import Message from 'primevue/message'
import Dialog from 'primevue/dialog'
import Badge from 'primevue/badge'
import Toast from 'primevue/toast'

const userStore = useUserStore()
const showUpgradeDialog = ref(false)

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
  max-width: 800px;
  margin: 2rem auto;
  padding: 0 1rem;
}

.profile-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.profile-title {
  font-size: 1.5rem;
  font-weight: 600;
}

.profile-content {
  display: grid;
  grid-template-columns: 1fr;
  gap: 2rem;
}

@media (min-width: 768px) {
  .profile-content {
    grid-template-columns: 1fr 1fr;
  }
}

.user-info-section,
.subscription-section {
  padding: 1rem 0;
}

h3 {
  font-size: 1.2rem;
  font-weight: 600;
  margin-bottom: 1.5rem;
  padding-bottom: 0.5rem;
  border-bottom: 1px solid #e9ecef;
}

.form-field {
  margin-bottom: 1.5rem;
}

.form-field label {
  display: block;
  margin-bottom: 0.5rem;
  font-weight: 500;
}

.subscription-info {
  background-color: #f8f9fa;
  border-radius: 8px;
  padding: 1rem;
  margin-bottom: 1.5rem;
}

.subscription-type {
  font-size: 1.2rem;
  font-weight: 700;
  margin-bottom: 0.5rem;
}

.subscription-type.premium {
  color: #f59e0b;
}

.subscription-details {
  font-size: 0.9rem;
  color: #6c757d;
}

.subscription-limits {
  margin-bottom: 1.5rem;
}

.subscription-limits h4 {
  font-size: 1rem;
  font-weight: 600;
  margin-bottom: 1rem;
}

.limit-item {
  display: flex;
  justify-content: space-between;
  margin-bottom: 0.5rem;
  font-size: 0.9rem;
}

.limit-label {
  color: #6c757d;
}

.limit-value {
  font-weight: 600;
}

.limit-value.unlimited {
  color: #10b981;
}

.p-progressbar {
  height: 8px;
  margin-top: 0.5rem;
}

.p-progressbar.almost-full .p-progressbar-value {
  background-color: #f59e0b;
}

.subscription-actions {
  margin-top: 1.5rem;
}

.loading-container,
.error-container {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  padding: 2rem;
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
  margin-bottom: 0.5rem;
  display: flex;
  align-items: center;
}

.benefits-list li i {
  color: #10b981;
  margin-right: 0.5rem;
}

.pricing {
  margin-top: 1.5rem;
}

.price {
  font-size: 1.5rem;
  font-weight: 700;
  color: #1976d2;
}

.billing-note {
  font-size: 0.8rem;
  color: #6c757d;
  margin-top: 0.3rem;
}
</style>
