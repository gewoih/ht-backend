<template>
  <div class="habit-details-container">
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
</template>

<script setup lang="ts">
import { ref, onMounted, watch, computed } from 'vue'
import { loadHabitDetails } from '../../services/habits.service'
import ProgressSpinner from 'primevue/progressspinner'
import Message from 'primevue/message'
import Divider from 'primevue/divider'
import { HabitDetails } from '../../types/habit-details'

const props = defineProps<{
  habitId: string
  habitName: string
}>()

const details = ref<HabitDetails | null>(null)
const loading = ref(false)
const error = ref('')

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
</style>
