<template>
  <div class="insights-dashboard">
    <div v-if="loading" class="loading">
      <i class="pi pi-spin pi-spinner" style="font-size: 2rem"></i>
      <div>Загрузка...</div>
    </div>
    <div v-else-if="error" class="error">
      <i class="pi pi-exclamation-triangle" style="font-size: 2rem; color: #ef4444"></i>
      <div>{{ error }}</div>
    </div>
    <div v-else-if="!insights.length" class="empty-insights">
      <div class="empty-message">
        <i class="pi pi-chart-bar empty-icon"></i>
        <h3>Аналитика скоро появится</h3>
        <p>
          Продолжайте заполнять дневник привычек, чтобы получить персональные инсайты о влиянии
          привычек на ваше самочувствие
        </p>
        <Button
          label="Заполнить дневник"
          icon="pi pi-pencil"
          class="fill-journal-button p-button-primary"
          @click="$router.push('/journal')"
        />
      </div>
    </div>
    <div v-else class="insights-content">
      <h2 class="dashboard-title">Влияние ваших привычек</h2>

      <!-- Positive habits section -->
      <div v-if="positiveInsights.length" class="habits-section positive-section">
        <h3 class="section-title positive"><i class="pi pi-arrow-up"></i> Полезные привычки</h3>
        <div v-for="item in positiveInsights" :key="item.habit.id" class="insight-card">
          <div class="insight-header">
            <span class="habit-name">{{ item.habit.name }}</span>
            <span class="influence-value positive"> +{{ item.influence.toFixed(1) }}% </span>
          </div>
          <div class="bar-center-container">
            <div class="bar-bg"></div>
            <div
              class="bar bar-positive"
              :style="{ width: item.normalized * 50 + '%', left: '50%' }"
            >
              <span class="bar-knob"></span>
            </div>
          </div>
        </div>
      </div>

      <!-- Negative habits section -->
      <div v-if="negativeInsights.length" class="habits-section negative-section">
        <h3 class="section-title negative"><i class="pi pi-arrow-down"></i> Вредные привычки</h3>
        <div v-for="item in negativeInsights" :key="item.habit.id" class="insight-card">
          <div class="insight-header">
            <span class="habit-name">{{ item.habit.name }}</span>
            <span class="influence-value negative"> {{ item.influence.toFixed(1) }}% </span>
          </div>
          <div class="bar-center-container">
            <div class="bar-bg"></div>
            <div
              class="bar bar-negative"
              :style="{
                width: Math.abs(item.normalized) * 50 + '%',
                left: 50 - Math.abs(item.normalized) * 50 + '%',
              }"
            ></div>
            <span
              class="bar-knob negative-knob"
              :style="{ left: 50 - Math.abs(item.normalized) * 50 + '%' }"
            >
            </span>
          </div>
        </div>
      </div>

      <!-- Neutral habits section -->
      <div v-if="neutralInsights.length" class="habits-section neutral-section">
        <h3 class="section-title neutral"><i class="pi pi-minus"></i> Нейтральные привычки</h3>
        <div v-for="item in neutralInsights" :key="item.habit.id" class="insight-card">
          <div class="insight-header">
            <span class="habit-name">{{ item.habit.name }}</span>
            <span class="influence-value neutral"> {{ item.influence.toFixed(1) }}% </span>
          </div>
          <div class="bar-center-container">
            <div class="bar-bg"></div>
            <div class="bar bar-zero" :style="{ left: '50%' }">
              <span class="bar-knob"></span>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, computed } from 'vue'
import { fetchInsights } from '../services/insights.service'
import Button from 'primevue/button'
import { useRouter } from 'vue-router'

interface Habit {
  id: string
  name: string
}

interface Insight {
  habit: Habit
  influence: number
}

const router = useRouter()

const insights = ref<Insight[]>([])
const loading = ref(true)
const error = ref('')

onMounted(async () => {
  try {
    insights.value = await fetchInsights()
  } catch (e: any) {
    error.value = e.message || 'Неизвестная ошибка'
  } finally {
    loading.value = false
  }
})

const normalizedInsights = computed(() => {
  if (!insights.value.length) return []
  const influences = insights.value.map((i) => i.influence)
  const maxAbs = Math.max(Math.abs(Math.max(...influences)), Math.abs(Math.min(...influences))) || 1
  return insights.value.map((i) => ({
    ...i,
    normalized: i.influence / maxAbs,
  }))
})

const sortedInsights = computed(() => {
  return [...normalizedInsights.value].sort((a, b) => b.influence - a.influence)
})

const positiveInsights = computed(() => {
  return sortedInsights.value.filter((insight) => insight.influence > 0)
})

const negativeInsights = computed(() => {
  return sortedInsights.value.filter((insight) => insight.influence < 0)
})

const neutralInsights = computed(() => {
  return sortedInsights.value.filter((insight) => insight.influence === 0)
})
</script>

<style scoped>
.insights-dashboard {
  max-width: 650px;
  margin: 2rem auto;
  padding: 1.5rem;
  background: #fff;
  border-radius: 16px;
  box-shadow: 0 4px 20px rgba(0, 0, 0, 0.06);
}

.insights-content {
  animation: fadeIn 0.5s ease-out;
}

.dashboard-title {
  color: #1e293b;
  text-align: center;
  font-size: 1.4rem;
  font-weight: 700;
  margin-bottom: 1.5rem;
}

.habits-section {
  margin-bottom: 2rem;
}

.section-title {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  font-size: 1.1rem;
  font-weight: 600;
  margin-bottom: 1rem;
  padding-bottom: 0.5rem;
  border-bottom: 1px solid #e2e8f0;
}

.section-title.positive {
  color: #10b981;
}

.section-title.negative {
  color: #f97316;
}

.section-title.neutral {
  color: #64748b;
}

.annotation {
  display: flex;
  justify-content: center;
  gap: 3rem;
  align-items: center;
  margin-bottom: 2rem;
}

.annotation-item {
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.indicator {
  width: 14px;
  height: 14px;
  border-radius: 50%;
}

.indicator.positive {
  background-color: #10b981;
}

.indicator.negative {
  background-color: #f97316;
}

.icon-label {
  font-size: 0.9rem;
  font-weight: 500;
  color: #64748b;
}

.insight-card {
  background: #f8fafc;
  border-radius: 12px;
  padding: 1.2rem 1.5rem 1rem;
  margin-bottom: 1.2rem;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.03);
  transition: transform 0.2s, box-shadow 0.2s;
}

.insight-card:hover {
  transform: translateY(-2px);
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.08);
}

.insight-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 0.8rem;
}

.habit-name {
  color: #1e293b;
  font-size: 1rem;
  font-weight: 600;
  letter-spacing: 0.02em;
}

.influence-value {
  font-size: 1.2rem;
  font-weight: 700;
  min-width: 54px;
  text-align: right;
  border-radius: 6px;
  padding: 0.2rem 0.5rem;
}

.influence-value.positive {
  color: #10b981;
  background: rgba(16, 185, 129, 0.1);
}

.influence-value.negative {
  color: #f97316;
  background: rgba(249, 115, 22, 0.1);
}

.influence-value.neutral {
  color: #64748b;
  background: rgba(100, 116, 139, 0.1);
}

.bar-center-container {
  position: relative;
  background: transparent;
  border-radius: 6px;
  height: 10px;
  margin: 0.8rem 0 0.4rem 0;
}

.bar-bg {
  position: absolute;
  left: 0;
  right: 0;
  top: 0;
  bottom: 0;
  background: #e2e8f0;
  border-radius: 6px;
  opacity: 1;
  z-index: 0;
}

.bar {
  position: absolute;
  top: 0;
  height: 100%;
  border-radius: 6px;
  z-index: 2;
  display: flex;
  align-items: center;
  transition: width 0.5s ease-out, left 0.5s ease-out;
}

.bar-positive {
  background: linear-gradient(90deg, #10b981, #34d399);
}

.bar-negative {
  background: linear-gradient(270deg, #f97316, #fb923c);
}

.bar-zero {
  background: #94a3b8;
  width: 3px;
}

.bar-knob {
  width: 18px;
  height: 18px;
  border-radius: 50%;
  background: #fff;
  border: 3px solid #cbd5e1;
  box-shadow: 0 2px 6px rgba(0, 0, 0, 0.1);
  margin-left: auto;
  margin-right: -9px;
  z-index: 3;
}

.negative-knob {
  position: absolute;
  top: -4px;
  transform: translateX(-50%);
  margin: 0;
}

.loading,
.error {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  gap: 1rem;
  text-align: center;
  margin: 3rem 0;
  color: #64748b;
}

.empty-insights {
  padding: 3rem 1.5rem;
  text-align: center;
}

.empty-message {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  gap: 1rem;
  max-width: 450px;
  margin: 0 auto;
}

.empty-icon {
  font-size: 3.5rem;
  color: #cbd5e1;
  margin-bottom: 0.5rem;
}

.empty-message h3 {
  font-size: 1.5rem;
  color: #1e293b;
  margin: 0;
}

.empty-message p {
  color: #64748b;
  line-height: 1.6;
  margin-bottom: 1rem;
}

.fill-journal-button {
  margin-top: 1rem;
}

@keyframes fadeIn {
  from {
    opacity: 0;
    transform: translateY(10px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

@media (max-width: 480px) {
  .insights-dashboard {
    margin: 1rem auto;
    padding: 1.2rem;
    border-radius: 12px;
  }

  .dashboard-title {
    font-size: 1.2rem;
  }

  .section-title {
    font-size: 1rem;
  }

  .empty-insights {
    padding: 2rem 1rem;
  }

  .empty-message h3 {
    font-size: 1.3rem;
  }

  .empty-message p {
    font-size: 0.9rem;
  }

  .insight-card {
    padding: 1rem 1.2rem 0.8rem;
  }
}
</style>
