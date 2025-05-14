<script setup lang="ts">
import { ref, onMounted, onBeforeUnmount, watch, nextTick, computed, onUpdated } from 'vue'
import { analyticsService } from '../services/analytics.service'
import { Chart, registerables } from 'chart.js'
import { format } from 'date-fns'
import SelectButton from 'primevue/selectbutton'
import Button from 'primevue/button'
import ProgressSpinner from 'primevue/progressspinner'
import type { AnalyticsData } from '../types/analytics'
import annotationPlugin from 'chartjs-plugin-annotation'
import { ru } from 'date-fns/locale'

Chart.register(annotationPlugin)
Chart.register(...registerables)

const chartCanvas = ref<HTMLCanvasElement | null>(null)
const analyticsData = ref<AnalyticsData>([])
const loading = ref(true)
const error = ref<string | null>(null)
let chart: Chart | null = null
const chartType = ref<'bar' | 'line'>('bar')

const dateRangeOptions = [
  { label: 'Текущая неделя', value: 'week' },
  { label: 'Текущий месяц', value: 'month' },
  { label: 'Текущий год', value: 'year' },
]
const selectedDateRange = ref<'week' | 'month' | 'year'>('week')

const averageScore = computed(() => {
  const entries = analyticsData.value
  const total = entries.length

  if (total === 0) return null

  const sum = entries.reduce(
    (acc, entry) => {
      acc.health += entry.score.health
      acc.energy += entry.score.energy
      acc.mood += entry.score.mood
      acc.sleep += entry.score.sleep
      acc.calmness += entry.score.calmness
      acc.satisfaction += entry.score.satisfaction
      return acc
    },
    { health: 0, energy: 0, mood: 0, sleep: 0, calmness: 0, satisfaction: 0 }
  )

  const avgHealth = sum.health / total
  const avgEnergy = sum.energy / total
  const avgMood = sum.mood / total
  const avgSleep = sum.sleep / total
  const avgCalmness = sum.calmness / total
  const avgSatisfaction = sum.satisfaction / total

  const overall = (avgHealth + avgEnergy + avgMood + avgSleep + avgCalmness + avgSatisfaction) / 6

  return {
    health: avgHealth.toFixed(1),
    energy: avgEnergy.toFixed(1),
    mood: avgMood.toFixed(1),
    sleep: avgSleep.toFixed(1),
    calmness: avgCalmness.toFixed(1),
    satisfaction: avgSatisfaction.toFixed(1),
    overall: overall.toFixed(1),
  }
})

const summary = computed(() => {
  if (analyticsData.value.length === 0) return null

  const scores = analyticsData.value.map((entry) => ({
    date: new Date(entry.date),
    score:
      (entry.score.health +
        entry.score.energy +
        entry.score.mood +
        entry.score.sleep +
        entry.score.calmness +
        entry.score.satisfaction) /
      6,
  }))

  const sorted = [...scores].sort((a, b) => b.score - a.score)

  const best = sorted[0]
  const worst = sorted[sorted.length - 1]
  const delta = sorted[0].score - sorted[sorted.length - 1].score
  const avgScore = scores.reduce((sum, item) => sum + item.score, 0) / scores.length

  let trend = 'стабильная'
  if (delta >= 1.5) trend = 'переменчивая'
  else if (delta >= 0.8) trend = 'умеренно изменчивая'

  // Calculate if the overall trend is improving, declining or stable
  const firstHalf = scores.slice(0, Math.floor(scores.length / 2))
  const secondHalf = scores.slice(Math.floor(scores.length / 2))

  const firstHalfAvg = firstHalf.reduce((sum, item) => sum + item.score, 0) / firstHalf.length
  const secondHalfAvg = secondHalf.reduce((sum, item) => sum + item.score, 0) / secondHalf.length

  const trendDirection = secondHalfAvg - firstHalfAvg

  return {
    bestDay: format(best.date, 'd MMMM', { locale: ru }),
    worstDay: format(worst.date, 'd MMMM', { locale: ru }),
    trend,
    avgScore,
    trendDirection,
  }
})

// Helper functions for the UI
function getMetricsData() {
  if (!averageScore.value) return {}
  return {
    health: averageScore.value.health,
    energy: averageScore.value.energy,
    mood: averageScore.value.mood,
    sleep: averageScore.value.sleep,
    calmness: averageScore.value.calmness,
    satisfaction: averageScore.value.satisfaction,
  }
}

function getMetricLabel(key: string): string {
  const labels = {
    health: 'Здоровье',
    energy: 'Энергия',
    mood: 'Настроение',
    sleep: 'Сон',
    calmness: 'Спокойствие',
    satisfaction: 'Удовлетворённость',
  }
  return labels[key as keyof typeof labels] || key
}

function getMetricIcon(key: string): string {
  const icons = {
    health: 'pi pi-heart-fill',
    energy: 'pi pi-bolt',
    mood: 'pi pi-thumbs-up',
    sleep: 'pi pi-moon',
    calmness: 'pi pi-cloud',
    satisfaction: 'pi pi-check-circle',
  }
  return icons[key as keyof typeof icons] || 'pi pi-circle'
}

function getMetricColorClass(value: number): string {
  if (value >= 4) return 'good'
  if (value >= 2.5) return 'average'
  return 'poor'
}

function getTrendClass(): string {
  if (!summary.value) return 'neutral'

  if (summary.value.trendDirection > 0.3) return 'positive'
  if (summary.value.trendDirection < -0.3) return 'negative'
  return 'neutral'
}

function getTrendIcon(): string {
  if (!summary.value) return 'pi pi-minus'

  if (summary.value.trendDirection > 0.3) return 'pi pi-arrow-up'
  if (summary.value.trendDirection < -0.3) return 'pi pi-arrow-down'
  return 'pi pi-minus'
}

function getTrendLabel(): string {
  if (!summary.value) return 'Нет данных'

  if (summary.value.trendDirection > 0.3) return 'Улучшение'
  if (summary.value.trendDirection < -0.3) return 'Снижение'
  return 'Стабильно'
}

// Function to calculate the start date of the current week (Monday)
function getCurrentWeekStart() {
  const today = new Date()
  const day = today.getDay() // 0 is Sunday, 1 is Monday
  // Calculate days to subtract to get to Monday (in Europe/Russia Monday is first day)
  const daysToSubtract = day === 0 ? 6 : day - 1
  const monday = new Date(today)
  monday.setDate(today.getDate() - daysToSubtract)
  monday.setHours(0, 0, 0, 0)
  return monday
}

// Function to calculate the start date of the current month
function getCurrentMonthStart() {
  const today = new Date()
  return new Date(today.getFullYear(), today.getMonth(), 1)
}

// Function to calculate the start date of the current year
function getCurrentYearStart() {
  const today = new Date()
  return new Date(today.getFullYear(), 0, 1)
}

// Updated getDateRange function to use current periods
function getDateRange(option: string) {
  const today = new Date()
  const toDate = format(today, 'yyyy-MM-dd')

  let fromDate
  if (option === 'week') {
    fromDate = format(getCurrentWeekStart(), 'yyyy-MM-dd')
  } else if (option === 'month') {
    fromDate = format(getCurrentMonthStart(), 'yyyy-MM-dd')
  } else {
    // year
    fromDate = format(getCurrentYearStart(), 'yyyy-MM-dd')
  }

  return { fromDate, toDate }
}

// Add a flag to track if we're currently rendering a chart
const isChartRendering = ref(false)

// Add a reference to the chart container
const chartContainerRef = ref<HTMLElement | null>(null)

// Function to check if chart container is visible and has dimensions
function isChartContainerReady(): boolean {
  if (!chartContainerRef.value) return false

  const { width, height } = chartContainerRef.value.getBoundingClientRect()
  return width > 0 && height > 0
}

// Update the renderChart function to check container dimensions
async function renderChart() {
  // Don't try to render if we're already in the middle of rendering
  if (isChartRendering.value) {
    console.log('Chart rendering already in progress, skipping')
    return
  }

  isChartRendering.value = true

  try {
    // Wait for next DOM update to ensure canvas is mounted
    await nextTick()

    // Ensure canvas element exists
    if (!chartCanvas.value) {
      console.error('Chart canvas element not found')
      return
    }

    // Check if the chart container is ready with dimensions
    if (!isChartContainerReady()) {
      console.warn('Chart container not ready or has zero dimensions')

      // Try again after a short delay
      setTimeout(() => {
        isChartRendering.value = false
        renderChart()
      }, 100)
      return
    }

    // Check if we have data to display
    if (!analyticsData.value || analyticsData.value.length === 0) {
      console.warn('No data available for chart')
      return
    }

    // Get the canvas context
    const ctx = chartCanvas.value.getContext('2d')
    if (!ctx) {
      console.error('Failed to get canvas 2D context')
      return
    }

    // Clean up any existing chart
    if (chart) {
      chart.destroy()
      chart = null
    }

    // Create data for the chart
    const data = getChartData()

    // Create the chart with appropriate type and options
    chart = new Chart(ctx, {
      type: chartType.value,
      data,
      options: getChartOptions(),
    })

    console.log('Chart successfully rendered')
  } catch (error) {
    console.error('Error rendering chart:', error)
  } finally {
    isChartRendering.value = false
  }
}

// Improve the period change handler
watch(selectedDateRange, async (newValue) => {
  // Destroy existing chart before fetching new data
  if (chart) {
    chart.destroy()
    chart = null
  }

  // Fetch new data for the selected period
  await fetchAnalytics()
})

// Update the fetchAnalytics function
async function fetchAnalytics() {
  loading.value = true
  error.value = null

  try {
    // Get date range based on selected period
    const { fromDate, toDate } = getDateRange(selectedDateRange.value)

    // Load data from API
    analyticsData.value = await analyticsService.getAnalytics(fromDate, toDate)

    // Wait for the DOM to update
    await nextTick()

    // Only try to render chart if we have data
    if (analyticsData.value.length > 0) {
      // Add slight delay to ensure DOM is fully updated
      setTimeout(() => {
        renderChart()
      }, 50)
    }
  } catch (err) {
    console.error('Failed to load analytics data:', err)
    error.value = 'Не удалось загрузить данные. Попробуйте позже.'
  } finally {
    loading.value = false
  }
}

function formatDateLabel(dateString: string): string {
  const date = new Date(dateString)
  return new Intl.DateTimeFormat('ru-RU', {
    day: 'numeric',
    month: selectedDateRange.value === 'year' ? 'short' : 'long',
  }).format(date)
}

function getChartData() {
  const sorted = [...analyticsData.value].sort(
    (a, b) => new Date(a.date).getTime() - new Date(b.date).getTime()
  )

  return {
    labels: sorted.map((d) => formatDateLabel(d.date)),
    datasets: [
      {
        label: 'Здоровье',
        backgroundColor: 'rgba(66, 165, 245, 0.7)',
        borderColor: '#42A5F5',
        data: sorted.map((d) => d.score.health),
        tension: 0.3,
      },
      {
        label: 'Энергия',
        backgroundColor: 'rgba(102, 187, 106, 0.7)',
        borderColor: '#66BB6A',
        data: sorted.map((d) => d.score.energy),
        tension: 0.3,
      },
      {
        label: 'Настроение',
        backgroundColor: 'rgba(255, 167, 38, 0.7)',
        borderColor: '#FFA726',
        data: sorted.map((d) => d.score.mood),
        tension: 0.3,
      },
      {
        label: 'Сон',
        backgroundColor: 'rgba(126, 87, 194, 0.7)',
        borderColor: '#7E57C2',
        data: sorted.map((d) => d.score.sleep),
        tension: 0.3,
      },
      {
        label: 'Спокойствие',
        backgroundColor: 'rgba(236, 64, 122, 0.7)',
        borderColor: '#EC407A',
        data: sorted.map((d) => d.score.calmness),
        tension: 0.3,
      },
      {
        label: 'Удовлетворённость',
        backgroundColor: 'rgba(38, 166, 154, 0.7)',
        borderColor: '#26A69A',
        data: sorted.map((d) => d.score.satisfaction),
        tension: 0.3,
      },
    ],
  }
}

const barChartOptions = {
  responsive: true,
  maintainAspectRatio: false,
  plugins: {
    legend: {
      position: 'top' as const,
      labels: { usePointStyle: true },
      display: true,
      align: 'center' as const,
    },
    tooltip: {
      callbacks: {
        label: function (context: any) {
          const value = context.parsed.y
          const label = context.dataset.label
          let mood = ''

          if (value >= 4) mood = 'Отлично'
          else if (value >= 3) mood = 'Нормально'
          else if (value >= 2) mood = 'Низко'
          else mood = 'Плохо'

          return `${label}: ${value} — ${mood}`
        },
      },
    },
  },
  scales: {
    x: {
      stacked: true,
      grid: {
        display: false,
      },
    },
    y: {
      stacked: true,
      min: 0,
      max: 30, // Increased max to accommodate 6 metrics stacked (6 × 5 = 30)
      ticks: {
        stepSize: 5,
        precision: 0,
      },
      grid: {
        color: 'rgba(0, 0, 0, 0.05)',
      },
    },
  },
  barPercentage: 0.8,
  categoryPercentage: 0.9,
}

const lineChartOptions = {
  responsive: true,
  maintainAspectRatio: false,
  plugins: {
    legend: {
      position: 'top' as const,
      labels: { usePointStyle: true },
      display: true,
      align: 'center' as const,
    },
    tooltip: {
      callbacks: {
        label: function (context: any) {
          const value = context.parsed.y
          const label = context.dataset.label
          let mood = ''

          if (value >= 4) mood = 'Отлично'
          else if (value >= 3) mood = 'Нормально'
          else if (value >= 2) mood = 'Низко'
          else mood = 'Плохо'

          return `${label}: ${value} — ${mood}`
        },
      },
    },
  },
  scales: {
    x: {
      grid: {
        display: false,
      },
    },
    y: {
      min: 0,
      max: 5,
      ticks: {
        stepSize: 1,
        precision: 0,
      },
      grid: {
        color: 'rgba(0, 0, 0, 0.05)',
      },
    },
  },
  elements: {
    line: {
      tension: 0.3,
      fill: false,
    },
    point: {
      radius: 4,
      hoverRadius: 6,
    },
  },
}

function getChartOptions() {
  return chartType.value === 'bar' ? barChartOptions : lineChartOptions
}

async function setChartType(type: 'bar' | 'line') {
  chartType.value = type
  // Destroy existing chart before recreating
  if (chart) {
    chart.destroy()
    chart = null
  }
  await nextTick()
  renderChart()
}

onMounted(async () => {
  console.log('Component mounted')
  await fetchAnalytics()
  console.log('Data fetched, chartCanvas available:', !!chartCanvas.value)
})

onBeforeUnmount(() => {
  if (chart) chart.destroy()
})

// Add a chart rendering watcher and onUpdated hook
onUpdated(async () => {
  if (analyticsData.value.length > 0 && chartCanvas.value && !chart) {
    await renderChart()
  }
})

// Ensure the chart panel is visible when there's data to show
function hasAnalyticsData() {
  return analyticsData.value && analyticsData.value.length > 0
}

// Add computed property for current date range display
const currentDateRangeText = computed(() => {
  const { fromDate, toDate } = getDateRange(selectedDateRange.value)
  const fromDateObj = new Date(fromDate)
  const toDateObj = new Date(toDate)

  if (selectedDateRange.value === 'week') {
    return `${format(fromDateObj, 'd MMMM', { locale: ru })} - ${format(toDateObj, 'd MMMM yyyy', {
      locale: ru,
    })}`
  } else if (selectedDateRange.value === 'month') {
    return format(fromDateObj, 'MMMM yyyy', { locale: ru })
  } else {
    // year
    return format(fromDateObj, 'yyyy', { locale: ru })
  }
})
</script>

<template>
  <div class="analytics-container">
    <div class="analytics-header">
      <div class="header-content">
        <h1>Моя аналитика</h1>
        <div class="date-range-display">
          <span class="date-range-period">{{ currentDateRangeText }}</span>
        </div>
        <p class="analytics-description">
          Отслеживайте динамику своего самочувствия и настроения с течением времени
        </p>
      </div>
      <div class="date-control">
        <label>Период:</label>
        <SelectButton
          v-model="selectedDateRange"
          :options="dateRangeOptions"
          option-label="label"
          option-value="value"
          class="date-selector"
        />
      </div>
    </div>

    <div v-if="loading" class="analytics-loading panel">
      <ProgressSpinner />
      <p>Загрузка данных аналитики...</p>
    </div>

    <div v-else-if="error" class="analytics-error panel">
      <i class="pi pi-exclamation-triangle error-icon"></i>
      <p>{{ error }}</p>
      <Button label="Повторить" icon="pi pi-refresh" @click="fetchAnalytics" />
    </div>

    <div v-else-if="analyticsData.length === 0" class="analytics-empty panel">
      <i class="pi pi-info-circle info-icon"></i>
      <p>Нет данных за выбранный период. Попробуйте выбрать другой диапазон.</p>
    </div>

    <div v-else class="analytics-dashboard">
      <!-- Merged Score Overview and Key Days Panel -->
      <div v-if="averageScore && summary" class="summary-panel panel">
        <div class="panel-header">
          <h2>Сводка периода</h2>
          <div class="score-trend-container">
            <span class="score-badge">{{ averageScore.overall }}/5</span>
            <span class="trend-pill" :class="getTrendClass()">
              <i :class="getTrendIcon()"></i>
              {{ getTrendLabel() }}
            </span>
          </div>
        </div>

        <div class="panel-divider"></div>

        <div class="highlights-content">
          <div class="highlight-item best-day">
            <i class="pi pi-chart-line highlight-icon"></i>
            <div class="highlight-details">
              <span class="highlight-label">Лучший день</span>
              <span class="highlight-value">{{ summary.bestDay }}</span>
            </div>
          </div>
          <div class="highlight-item worst-day">
            <i class="pi pi-chart-line highlight-icon"></i>
            <div class="highlight-details">
              <span class="highlight-label">Сложный день</span>
              <span class="highlight-value">{{ summary.worstDay }}</span>
            </div>
          </div>
        </div>
      </div>

      <!-- Metrics Grid -->
      <div class="metrics-panel panel">
        <div class="panel-header">
          <h2>Средние показатели</h2>
        </div>
        <div class="metrics-grid">
          <div
            v-for="(value, key) in getMetricsData()"
            :key="key"
            class="metric-item"
            :class="getMetricColorClass(Number(value))"
          >
            <div class="metric-icon-container">
              <i :class="getMetricIcon(key)"></i>
            </div>
            <div class="metric-details">
              <span class="metric-label">{{ getMetricLabel(key) }}</span>
              <span class="metric-value">{{ value }}</span>
            </div>
          </div>
        </div>
      </div>

      <!-- Chart Panel -->
      <div v-if="hasAnalyticsData()" class="chart-panel panel">
        <div class="panel-header">
          <h2>Динамика показателей</h2>
          <div class="chart-controls">
            <Button
              :class="{ active: chartType === 'bar' }"
              icon="pi pi-chart-bar"
              text
              rounded
              @click="setChartType('bar')"
              v-tooltip.top="'Столбчатая диаграмма'"
            />
            <Button
              :class="{ active: chartType === 'line' }"
              icon="pi pi-chart-line"
              text
              rounded
              @click="setChartType('line')"
              v-tooltip.top="'Линейная диаграмма'"
            />
          </div>
        </div>
        <div class="chart-container" ref="chartContainerRef">
          <div v-if="loading" class="chart-loading">
            <ProgressSpinner />
            <p>Загрузка графика...</p>
          </div>
          <canvas ref="chartCanvas" id="analytics-chart" :key="selectedDateRange"></canvas>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
/* Base Styles */
.analytics-container {
  max-width: 1200px;
  margin: 0 auto;
  padding: 1rem;
}

/* Header Styles */
.analytics-header {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
  margin-bottom: 1.5rem;
  flex-wrap: wrap;
  gap: 1rem;
}

.header-content h1 {
  margin: 0 0 0.5rem 0;
  color: var(--primary-color, #4318ff);
  font-size: 1.8rem;
}

.analytics-description {
  color: var(--text-color-secondary, #8f9bba);
  margin: 0;
}

.date-control {
  display: flex;
  align-items: center;
  gap: 0.8rem;
}

.date-selector :deep(.p-button) {
  padding: 0.5rem 1rem;
}

/* Dashboard Layout */
.analytics-dashboard {
  display: grid;
  grid-template-columns: repeat(12, 1fr);
  gap: 1rem;
}

.summary-panel {
  grid-column: span 4;
}

.metrics-panel {
  grid-column: span 8;
}

.chart-panel {
  grid-column: span 12;
}

/* Panel divider */
.panel-divider {
  height: 1px;
  background-color: rgba(0, 0, 0, 0.1);
  margin: 1.2rem 0;
  width: 100%;
}

/* Panel Styles */
.panel {
  background-color: white;
  border-radius: 12px;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.05);
  padding: 1.5rem;
  display: flex;
  flex-direction: column;
}

.panel-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 1rem;
}

.panel-header h2 {
  margin: 0;
  font-size: 1.2rem;
  color: var(--primary-color, #4318ff);
  font-weight: 600;
}

/* Score Overview Panel */
.score-trend-container {
  display: flex;
  align-items: center;
  gap: 0.8rem;
}

.score-badge {
  font-size: 1.5rem;
  font-weight: 700;
  color: var(--primary-color, #4318ff);
  padding: 0.4rem 0.8rem;
  background-color: rgba(67, 24, 255, 0.1);
  border-radius: 8px;
  white-space: nowrap;
}

.trend-pill {
  display: flex;
  align-items: center;
  font-size: 0.9rem;
  padding: 0.3rem 0.6rem;
  border-radius: 12px;
  white-space: nowrap;
}

.trend-pill i {
  margin-right: 0.4rem;
}

.trend-pill.positive {
  color: white;
  background-color: #22c55e;
}

.trend-pill.negative {
  color: white;
  background-color: #ef4444;
}

.trend-pill.neutral {
  color: white;
  background-color: #8f9bba;
}

/* Metrics Grid */
.metrics-grid {
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  gap: 1rem;
}

.metric-item {
  display: flex;
  padding: 1rem;
  border-radius: 8px;
  box-shadow: 0 2px 5px rgba(0, 0, 0, 0.05);
  align-items: center;
  gap: 0.8rem;
  transition: transform 0.2s, box-shadow 0.2s;
}

.metric-item:hover {
  transform: translateY(-2px);
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.08);
}

.metric-icon-container {
  width: 2.5rem;
  height: 2.5rem;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 1.2rem;
  color: white;
}

.metric-details {
  display: flex;
  flex-direction: column;
}

.metric-label {
  font-size: 0.9rem;
  color: #666;
}

.metric-value {
  font-size: 1.3rem;
  font-weight: 600;
}

/* Metric Colors */
.metric-item.good {
  background-color: rgba(34, 197, 94, 0.1);
  border-left: 3px solid #22c55e;
}
.metric-item.good .metric-icon-container {
  background-color: #22c55e;
}
.metric-item.good .metric-value {
  color: #16a34a;
  font-weight: 700;
}

.metric-item.average {
  background-color: rgba(245, 158, 11, 0.1);
  border-left: 3px solid #f59e0b;
}
.metric-item.average .metric-icon-container {
  background-color: #f59e0b;
}
.metric-item.average .metric-value {
  color: #d97706;
  font-weight: 700;
}

.metric-item.poor {
  background-color: rgba(239, 68, 68, 0.1);
  border-left: 3px solid #ef4444;
}
.metric-item.poor .metric-icon-container {
  background-color: #ef4444;
}
.metric-item.poor .metric-value {
  color: #dc2626;
  font-weight: 700;
}

/* Highlights Panel */
.highlights-content {
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

.highlight-item {
  display: flex;
  align-items: center;
  padding: 1rem;
  border-radius: 8px;
  gap: 1rem;
}

.best-day {
  background-color: rgba(34, 197, 94, 0.1);
}
.best-day .highlight-icon {
  color: #16a34a;
}
.best-day .highlight-value {
  color: #16a34a;
}

.worst-day {
  background-color: rgba(239, 68, 68, 0.1);
}
.worst-day .highlight-icon {
  color: #dc2626;
}
.worst-day .highlight-value {
  color: #dc2626;
}

.highlight-icon {
  font-size: 1.5rem;
}

.highlight-details {
  display: flex;
  flex-direction: column;
}

.highlight-label {
  font-size: 0.9rem;
  color: #666;
}

.highlight-value {
  font-size: 1.1rem;
  font-weight: 600;
}

/* Chart Panel */
.chart-controls {
  display: flex;
  gap: 0.5rem;
}

.chart-controls .active {
  color: var(--primary-color, #4318ff);
  background-color: rgba(67, 24, 255, 0.1);
}

.chart-container {
  flex: 1;
  height: 400px;
  min-height: 400px;
  position: relative;
  width: 100%;
}

/* Status Panels */
.analytics-loading,
.analytics-error,
.analytics-empty {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  min-height: 400px;
  text-align: center;
  gap: 1rem;
}

.error-icon,
.info-icon {
  font-size: 2.5rem;
  margin-bottom: 1rem;
}

.error-icon {
  color: #ef4444;
}

.info-icon {
  color: #3b82f6;
}

/* Responsive Adjustments */
@media (max-width: 992px) {
  .summary-panel,
  .metrics-panel {
    grid-column: span 6;
  }

  .metrics-grid {
    grid-template-columns: repeat(2, 1fr);
  }
}

@media (max-width: 768px) {
  .analytics-header {
    flex-direction: column;
    align-items: flex-start;
  }

  .summary-panel,
  .metrics-panel,
  .chart-panel {
    grid-column: span 12;
  }
}

@media (max-width: 576px) {
  .metrics-grid {
    grid-template-columns: 1fr;
  }

  .panel-header {
    flex-direction: column;
    align-items: flex-start;
    gap: 0.8rem;
  }

  .score-trend-container {
    width: 100%;
    justify-content: space-between;
  }
}

/* Chart Loading State */
.chart-loading {
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  background-color: rgba(255, 255, 255, 0.8);
  z-index: 2;
}

/* Date Range Display Styles */
.date-range-display {
  margin-bottom: 0.5rem;
}

.date-range-period {
  font-size: 1.1rem;
  font-weight: 500;
  color: var(--primary-color, #4318ff);
  opacity: 0.9;
}
</style>
