<template>
  <div class="insights-dashboard">
    <h2 class="dashboard-title">Влияние ваших привычек (%)</h2>
    <div class="annotation">
      <span class="icon-hurt">
        <svg
          width="18"
          height="18"
          viewBox="0 0 18 18"
          fill="none"
          xmlns="http://www.w3.org/2000/svg"
        >
          <path d="M9 15L3 6H15L9 15Z" fill="#ffb300" />
        </svg>
        <span class="icon-label hurt-label">Вредит</span>
      </span>
      <span class="icon-help">
        <svg
          width="18"
          height="18"
          viewBox="0 0 18 18"
          fill="none"
          xmlns="http://www.w3.org/2000/svg"
        >
          <path d="M9 3L15 12H3L9 3Z" fill="#22c55e" />
        </svg>
        <span class="icon-label help-label">Помогает</span>
      </span>
    </div>
    <div v-if="loading" class="loading">Загрузка...</div>
    <div v-else-if="error" class="error">Ошибка: {{ error }}</div>
    <div v-else>
      <div v-for="item in normalizedInsights" :key="item.habit.id" class="insight-card">
        <div class="insight-header">
          <span class="habit-name">{{ item.habit.name.toUpperCase() }}</span>
          <span
            class="influence-value"
            :class="{ positive: item.influence > 0, negative: item.influence < 0 }"
          >
            {{ item.influence > 0 ? '+' : '' }}{{ item.influence.toFixed(0) }}%
          </span>
        </div>
        <div class="bar-center-container">
          <div class="bar-bg"></div>
          <div
            v-if="item.influence < 0"
            class="bar bar-negative"
            :style="{
              width: Math.abs(item.normalized) * 50 + '%',
              left: 50 - Math.abs(item.normalized) * 50 + '%',
            }"
          >
            <span class="bar-knob"></span>
          </div>
          <div
            v-else-if="item.influence > 0"
            class="bar bar-positive"
            :style="{ width: item.normalized * 50 + '%', left: '50%' }"
          >
            <span class="bar-knob"></span>
          </div>
          <div v-else class="bar bar-zero" :style="{ left: '50%' }">
            <span class="bar-knob"></span>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, computed } from 'vue'
import { fetchInsights } from '../services/insights.service'

interface Habit {
  id: string
  name: string
}

interface Insight {
  habit: Habit
  influence: number
}

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
</script>

<style scoped>
.insights-dashboard {
  max-width: 600px;
  margin: 2rem auto;
  padding: 1rem;
  background: #fff;
  border-radius: 12px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.05);
}
.dashboard-title {
  color: #222;
  text-align: center;
  font-size: 1.2rem;
  font-weight: 700;
  letter-spacing: 0.1em;
  margin-bottom: 0.5rem;
}
.annotation {
  display: flex;
  justify-content: center;
  gap: 2.5rem;
  align-items: flex-end;
  margin-bottom: 1.5rem;
}
.icon-help,
.icon-hurt {
  display: flex;
  flex-direction: column;
  align-items: center;
  font-size: 0.8rem;
}
.icon-label {
  margin-top: 0.1rem;
  font-size: 0.8rem;
  color: #888;
  letter-spacing: 0.04em;
}
.help-label {
  color: #22c55e;
}
.hurt-label {
  color: #ffb300;
}
.insight-card {
  background: #f8f9fa;
  border-radius: 10px;
  padding: 1rem 1.2rem 0.7rem 1.2rem;
  margin-bottom: 1.2rem;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.04);
}
.insight-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 0.5rem;
}
.habit-name {
  color: #222;
  font-size: 0.95rem;
  font-weight: 700;
  letter-spacing: 0.04em;
  text-transform: uppercase;
}
.influence-value {
  font-size: 1.1rem;
  font-weight: 700;
  min-width: 48px;
  text-align: right;
}
.influence-value.positive {
  color: #22c55e;
}
.influence-value.negative {
  color: #ffb300;
}
.bar-center-container {
  position: relative;
  background: transparent;
  border-radius: 4px;
  height: 8px;
  margin: 0.5rem 0 0.2rem 0;
}
.bar-bg {
  position: absolute;
  left: 0;
  right: 0;
  top: 0;
  bottom: 0;
  background: repeating-linear-gradient(
    90deg,
    #e9ecef 0 4px,
    #e9ecef 8px,
    #f8f9fa 8px,
    #f8f9fa 12px
  );
  border-radius: 4px;
  opacity: 1;
  z-index: 0;
}
.bar {
  position: absolute;
  top: 0;
  height: 100%;
  border-radius: 4px;
  z-index: 2;
  display: flex;
  align-items: center;
  transition: width 0.3s, left 0.3s;
}
.bar-positive {
  background: #22c55e;
}
.bar-negative {
  background: #ffb300;
}
.bar-zero {
  background: #bbb;
  width: 2px;
}
.bar-knob {
  width: 16px;
  height: 16px;
  border-radius: 50%;
  background: #fff;
  border: 3px solid #e9ecef;
  box-shadow: 0 2px 6px rgba(0, 0, 0, 0.1);
  margin-left: auto;
  margin-right: -8px;
  z-index: 3;
}
.loading,
.error {
  text-align: center;
  margin: 2rem 0;
}
</style>
