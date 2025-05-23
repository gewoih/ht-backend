<template>
  <div class="habit-details-container">
    <div v-if="isSubscriptionExpired" class="subscription-expired-overlay">
      <div class="expired-content">
        <i class="pi pi-lock expired-icon"></i>
        <h2>Подписка истекла</h2>
        <p>
          Доступ ко всем функциям временно ограничен. Пожалуйста, продлите подписку для продолжения
          использования сервиса.
        </p>
        <Button
          label="Продлить подписку"
          icon="pi pi-credit-card"
          class="p-button-lg"
          @click="goToProfile"
        />
      </div>
    </div>
    <div v-else>
      <div v-if="loading" class="details-loading">
        <ProgressSpinner />
        <p>Загрузка информации...</p>
      </div>
      <div v-else-if="error" class="details-error">
        <Message severity="error">{{ error }}</Message>
      </div>
      <div v-else-if="details" class="details-content">
        <h3>{{ habitName }}</h3>

        <div class="impact-indicator" :class="impactClass">
          <span>Влияние:</span>
          <strong>{{ details.impact }}</strong>
        </div>

        <div class="details-section">
          <h4>Описание</h4>
          <p>{{ details.description }}</p>
        </div>

        <Divider />

        <div class="details-section">
          <h4>Рекомендация</h4>
          <p>{{ details.recommendation }}</p>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, watch, computed } from 'vue'
import { loadHabitDetails } from '../../services/habits.service'
import ProgressSpinner from 'primevue/progressspinner'
import Message from 'primevue/message'
import Divider from 'primevue/divider'
import Button from 'primevue/button'
import { HabitDetails } from '../../types/habit-details'
import { useUserStore } from '../../stores/userStore'
import { storeToRefs } from 'pinia'
import { useRouter } from 'vue-router'

const props = defineProps<{
  habitId: string
  habitName: string
}>()

const details = ref<HabitDetails | null>(null)
const loading = ref(false)
const error = ref('')

const userStore = useUserStore()
const { isSubscriptionExpired } = storeToRefs(userStore)
const router = useRouter()
function goToProfile() {
  router.push('/profile')
}

async function fetchDetails() {
  if (!props.habitId) return

  loading.value = true
  details.value = null
  error.value = ''

  try {
    const habitDetails = await loadHabitDetails(props.habitId)
    details.value = {
      description: habitDetails.description,
      recommendation: habitDetails.recommendation,
      impact: habitDetails.impact,
    }
  } catch (e: any) {
    error.value = e.message || 'Ошибка при загрузке информации о привычке'
  } finally {
    loading.value = false
  }
}

onMounted(() => {
  fetchDetails()
})

watch(
  () => props.habitId,
  () => {
    fetchDetails()
  }
)

// Вычисляем CSS класс для влияния (цветовая шкала)
const impactClass = computed(() => {
  if (!details.value) return ''
  if (details.value.impact >= 4) return 'impact-positive-strong'
  if (details.value.impact >= 2) return 'impact-positive'
  if (details.value.impact <= -4) return 'impact-negative-strong'
  if (details.value.impact <= -2) return 'impact-negative'
  return 'impact-neutral'
})
</script>

<style scoped>
.habit-details-container {
  padding: 0.5rem;
}

.details-loading,
.details-error {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  padding: 2rem 1rem;
  gap: 1rem;
}

.details-content {
  display: flex;
  flex-direction: column;
  gap: 1.2rem;
}

.details-content h3 {
  margin: 0 0 0.5rem 0;
  font-size: 1.4rem;
  font-weight: 600;
  color: #1976d2;
  text-align: center;
}

.impact-indicator {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  gap: 0.4rem;
  padding: 0.4rem 0.8rem;
  border-radius: 0.4rem;
  font-size: 1rem;
  font-weight: 600;
  max-width: max-content;
  margin: 0 auto;
}

.impact-positive-strong {
  background-color: #e8f5e9;
  color: #2e7d32;
  border: 1px solid #81c784;
}

.impact-positive {
  background-color: #f1f8e9;
  color: #558b2f;
  border: 1px solid #aed581;
}

.impact-negative {
  background-color: #ffebee;
  color: #c62828;
  border: 1px solid #e57373;
}

.impact-negative-strong {
  background-color: #ffebee;
  color: #b71c1c;
  border: 1px solid #ef5350;
}

.impact-neutral {
  background-color: #f5f5f5;
  color: #555;
  border: 1px solid #ccc;
}

.details-section {
  padding: 0.5rem 0.2rem;
}

.details-section h4 {
  margin: 0 0 0.8rem 0;
  font-size: 1.1rem;
  font-weight: 600;
  color: #333;
  position: relative;
  padding-left: 0.8rem;
}

.details-section h4::before {
  content: '';
  position: absolute;
  left: 0;
  top: 0.2rem;
  bottom: 0.2rem;
  width: 4px;
  background-color: #1976d2;
  border-radius: 2px;
}

.details-section p {
  margin: 0;
  padding: 0 0.5rem;
  line-height: 1.5;
  color: #555;
  font-size: 1rem;
}

.subscription-expired-overlay {
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background: rgba(255, 255, 255, 0.96);
  z-index: 9999;
  display: flex;
  align-items: center;
  justify-content: center;
}
.expired-content {
  background: white;
  border-radius: 1.5rem;
  box-shadow: 0 8px 32px rgba(0, 0, 0, 0.12);
  padding: 3rem 2rem;
  text-align: center;
  max-width: 400px;
}
.expired-icon {
  font-size: 3rem;
  color: #1976d2;
  margin-bottom: 1.5rem;
}
</style>
