<script setup lang="ts">
import { onMounted, watch, computed, ref } from 'vue'
import Card from 'primevue/card'
import Button from 'primevue/button'
import Calendar from 'primevue/calendar'
import Checkbox from 'primevue/checkbox'
import ProgressSpinner from 'primevue/progressspinner'
import Message from 'primevue/message'
import { useJournalStore } from '../stores/journalStore'
import { useHabitStore } from '../stores/habitStore'
import type { Habit } from '../types/habit'
import ProgressBar from 'primevue/progressbar'
import ScoreWithInfo from '../components/ui/ScoreWithInfo.vue'
import { ScoreDescriptions } from '../data/score-descriptions'
import HabitsInfo from '../components/habits/HabitsInfo.vue'

const journalStore = useJournalStore()
const habitStore = useHabitStore()
const showHabitsDialog = ref(false)

const updateHabit = (habitId: string, value: boolean) => {
  const habit = journalStore.habits.find((h) => h.id === habitId)
  if (habit) habit.completed = value
}

const toggleHabit = (habit: Habit) => {
  updateHabit(habit.id, !habit.completed)
}

const groupedHabits = computed(() =>
  journalStore.habits.reduce((groups, habit) => {
    const cat = habit.category || 'Без категории'
    if (!groups[cat]) groups[cat] = []
    groups[cat].push(habit)
    return groups
  }, {} as Record<string, Habit[]>)
)

const totalPositiveImpact = computed(() => {
  return journalStore.habits.filter((h) => h.impact > 0).reduce((sum, h) => sum + h.impact, 0)
})

const completedImpact = computed(() => {
  return journalStore.habits.filter((h) => h.completed).reduce((sum, h) => sum + h.impact, 0)
})

const progressPercent = computed(() => {
  if (totalPositiveImpact.value === 0) return 0
  const percent = (completedImpact.value / totalPositiveImpact.value) * 100
  return Math.max(0, Math.round(percent))
})

const progressBarClass = computed(() => {
  if (progressPercent.value <= 33) return 'progress-danger'
  if (progressPercent.value <= 66) return 'progress-warning'
  return 'progress-success'
})

const updateScore = (key: string, value: number) => {
  journalStore.dailyScore[key] = value
}

const openHabitsSelection = () => {
  showHabitsDialog.value = true
}

const onHabitsUpdated = (updatedHabits) => {
  // Update local habits collection without a new server request
  if (updatedHabits && updatedHabits.length >= 0) {
    // Set all habits as not completed initially since it's a new selection
    const newHabits = updatedHabits.map((habit) => ({
      ...habit,
      completed: false,
    }))

    // Update the store with new habits directly
    journalStore.habits = newHabits
  }
}

watch(
  () => journalStore.selectedDate,
  () => {
    journalStore.loadData()
  },
  { immediate: true }
)
</script>

<template>
  <div class="habit-journal">
    <Card class="journal-card">
      <template #header>
        <div class="journal-header">
          <div class="journal-header-left">
            <h2 class="journal-title">Дневник привычек</h2>
          </div>
          <div class="journal-header-right">
            <div class="date-navigation">
              <Button
                icon="pi pi-chevron-left"
                @click="journalStore.goToPreviousDay"
                text
                rounded
                class="nav-button"
                v-tooltip.top="'Предыдущий день'"
              />
              <Calendar
                v-model="journalStore.selectedDate"
                dateFormat="dd.mm.yy"
                :showIcon="true"
                :maxDate="new Date()"
                class="date-picker"
                :showButtonBar="true"
                placeholder="Выберите дату"
              />
              <Button
                icon="pi pi-chevron-right"
                @click="journalStore.goToNextDay"
                text
                rounded
                :disabled="journalStore.isToday"
                class="nav-button"
                v-tooltip.top="journalStore.isToday ? 'Сегодня' : 'Следующий день'"
              />
            </div>
            <Button
              icon="pi pi-cog"
              text
              rounded
              severity="secondary"
              class="habits-settings-button"
              v-tooltip.top="'Настроить привычки'"
              @click="openHabitsSelection"
            />
          </div>
        </div>
      </template>

      <template #content>
        <div v-if="journalStore.isLoading" class="loading-container">
          <ProgressSpinner />
          <p>Загрузка данных...</p>
        </div>

        <div v-else-if="journalStore.hasError" class="error-container">
          <Message severity="error">{{ journalStore.errorMessage }}</Message>
          <Button
            label="Повторить"
            icon="pi pi-refresh"
            @click="journalStore.loadData"
            class="retry-button"
          />
        </div>

        <div v-else class="journal-content">
          <div class="progress-section">
            <div class="progress-info">
              <span class="progress-label">Прогресс дня</span>
              <span class="progress-value">{{ completedImpact }} из {{ totalPositiveImpact }}</span>
            </div>
            <ProgressBar
              :value="progressPercent"
              :class="progressBarClass"
              showValue
              class="impact-progress"
            />
          </div>

          <div class="habits-list">
            <div v-if="journalStore.habits.length === 0" class="no-habits">
              <Button
                icon="pi pi-plus"
                class="add-habits-button p-button-outlined"
                @click="openHabitsSelection"
              >
                <span class="add-habits-text">Добавить привычки</span>
              </Button>
            </div>
            <template v-else>
              <div
                v-for="(habits, category) in groupedHabits"
                :key="category"
                class="habit-category-group"
              >
                <div class="habit-category-label">
                  <i class="pi pi-tag category-icon"></i>
                  <span>{{ category }}</span>
                </div>

                <div class="habit-cards">
                  <div
                    v-for="habit in habits"
                    :key="habit.id"
                    class="habit-card"
                    :class="{ 'habit-completed': habit.completed }"
                    @click="toggleHabit(habit)"
                  >
                    <div class="habit-card-content">
                      <div class="habit-status">
                        <Checkbox
                          :modelValue="habit.completed"
                          @click.stop
                          @change="updateHabit(habit.id, !habit.completed)"
                          :binary="true"
                          :inputId="'habit-' + habit.id"
                        />
                      </div>
                      <div class="habit-info">
                        <label class="habit-label">
                          {{ habit.name }}
                        </label>
                      </div>
                      <div
                        class="habit-impact"
                        :class="
                          habit.impact > 0
                            ? 'positive-impact'
                            : habit.impact < 0
                            ? 'negative-impact'
                            : 'neutral-impact'
                        "
                      >
                        {{ habit.impact > 0 ? '+' + habit.impact : habit.impact }}
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </template>
          </div>

          <div class="scores-container">
            <h3 class="scores-title">Как прошел ваш день?</h3>

            <div class="scores-grid">
              <ScoreWithInfo
                v-for="(desc, key) in ScoreDescriptions"
                :key="key"
                :label="desc.label"
                :infoText="desc.text"
                :modelValue="journalStore.dailyScore[key]"
                :onUpdate="(n) => updateScore(key, n)"
                :scoreKey="key"
              />
            </div>

            <div v-if="journalStore.hasIncompleteScores" class="scores-warning">
              <i class="pi pi-exclamation-triangle"></i>
              <span>Пожалуйста, оцените все показатели перед сохранением</span>
            </div>

            <Button
              label="Сохранить"
              icon="pi pi-save"
              @click="journalStore.saveJournal"
              :disabled="journalStore.isSaving || journalStore.hasIncompleteScores"
              :loading="journalStore.isSaving"
              class="save-button"
              v-tooltip="
                journalStore.hasIncompleteScores
                  ? 'Необходимо оценить все показатели'
                  : 'Сохранить оценки и привычки'
              "
            />
          </div>
        </div>
      </template>
    </Card>

    <!-- Habits selection popup -->
    <HabitsInfo v-model:visible="showHabitsDialog" @habits-updated="onHabitsUpdated" />
  </div>
</template>

<style>
/* Base container styles */
.habit-journal {
  max-width: 800px;
  width: 100%;
  margin: 20px auto;
  padding: 20px;
}

/* Card styles */
.journal-card {
  margin-bottom: 1rem;
  border-radius: 12px;
  box-shadow: 0 2px 12px rgba(0, 0, 0, 0.07);
  overflow: hidden;
}

/* Header styles */
.journal-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 0.75rem 1rem;
  background-color: #f8f9fa;
  flex-wrap: wrap;
  gap: 1rem;
}

.journal-header-left {
  display: flex;
  align-items: center;
}

.journal-header-right {
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.journal-title {
  margin: 0;
  font-size: 1.25rem;
  color: #1976d2;
  font-weight: 600;
}

/* Date picker and navigation */
.date-navigation {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  background-color: white;
  border-radius: 8px;
  padding: 0.25rem;
  box-shadow: 0 1px 3px rgba(0, 0, 0, 0.05);
}

.nav-button {
  width: 2rem;
  height: 2rem;
  color: #1976d2;
}

.date-picker {
  width: 170px;
}

.date-picker :deep(.p-inputtext) {
  padding-right: 2.5rem;
  font-size: 0.9rem;
}

.habits-settings-button {
  width: 2.5rem;
  height: 2.5rem;
  min-width: 2.5rem;
}

/* Progress section */
.progress-section {
  background-color: #f8f9fa;
  border-radius: 10px;
  padding: 1rem;
  box-shadow: 0 1px 3px rgba(0, 0, 0, 0.05);
}

.progress-info {
  display: flex;
  justify-content: space-between;
  margin-bottom: 0.5rem;
  font-weight: 500;
}

.progress-label {
  color: #555;
}

.progress-value {
  color: #1976d2;
  font-weight: 600;
}

.impact-progress {
  height: 24px !important;
  font-weight: 700;
  font-size: 1rem;
  overflow: hidden;
  border-radius: 8px;
}

.p-progressbar {
  border-radius: 8px;
}

.p-progressbar .p-progressbar-value {
  display: flex;
  align-items: center;
  justify-content: center;
  transition: width 0.4s ease;
  color: white;
  font-weight: 700;
  font-size: 1rem;
  border-radius: 8px;
}

.progress-danger .p-progressbar-value {
  background-color: #ef4444 !important;
}
.progress-warning .p-progressbar-value {
  background-color: #eab308 !important;
}
.progress-success .p-progressbar-value {
  background-color: #22c55e !important;
}

/* Habits list */
.habits-list {
  margin-top: 1rem;
  max-height: 400px;
  overflow-y: auto;
  padding-right: 0.5rem;
  scrollbar-width: thin;
}

.habits-list::-webkit-scrollbar {
  width: 6px;
}

.habits-list::-webkit-scrollbar-thumb {
  background-color: rgba(0, 0, 0, 0.2);
  border-radius: 3px;
}

.habit-category-group {
  margin-bottom: 1.5rem;
}

.habit-category-label {
  display: flex;
  align-items: center;
  font-weight: 600;
  font-size: 1.1rem;
  margin-bottom: 0.8rem;
  color: #1976d2;
  gap: 0.5rem;
}

.category-icon {
  color: #1976d2;
  font-size: 0.9rem;
}

/* Habit cards */
.habit-cards {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(250px, 1fr));
  gap: 1rem;
}

.habit-card {
  background-color: #fff;
  border-radius: 12px;
  box-shadow: 0 2px 6px rgba(0, 0, 0, 0.05);
  padding: 1rem;
  transition: all 0.2s ease;
  border: 1px solid #f1f1f1;
  cursor: pointer;
  display: flex;
  align-items: center;
  min-height: 60px;
}

.habit-card:hover {
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
  transform: translateY(-2px);
}

.habit-completed {
  background-color: #f0f7ff;
  border-color: #c2e0ff;
}

.habit-card-content {
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: 0.8rem;
  width: 100%;
}

.habit-status {
  display: flex;
  align-items: center;
  justify-content: center;
}

.habit-info {
  flex: 1;
  display: flex;
  align-items: center;
}

.habit-label {
  cursor: pointer;
  margin: 0;
  font-weight: 500;
  color: #333;
  width: 100%;
  display: inline-block;
  padding: 0.25rem 0;
}

.habit-impact {
  min-width: 40px;
  text-align: center;
  font-weight: 600;
  font-size: 0.9rem;
  padding: 0.25rem 0.5rem;
  border-radius: 12px;
}

.positive-impact {
  color: #22c55e;
  background-color: rgba(34, 197, 94, 0.1);
}

.negative-impact {
  color: #ef4444;
  background-color: rgba(239, 68, 68, 0.1);
}

.neutral-impact {
  color: #999;
  background-color: rgba(0, 0, 0, 0.05);
}

/* Empty state */
.no-habits {
  display: flex;
  flex-direction: column;
  align-items: center;
  padding: 2rem;
  text-align: center;
  gap: 1rem;
}

.add-habits-button {
  padding: 0.75rem 1.5rem;
  width: 100%;
  max-width: 400px;
  font-size: 1rem;
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 0.75rem;
  border: 1px dashed #ccc;
  background-color: #f8f9fa;
  transition: all 0.2s;
}

.add-habits-button:hover {
  background-color: #e9ecef !important;
  border-color: #aaa !important;
}

.add-habits-text {
  font-weight: 600;
}

/* Scores section */
.scores-container {
  background-color: #f8f9fa;
  border-radius: 8px;
  padding: 1.5rem;
  margin-top: 1.5rem;
}

.scores-title {
  margin-top: 0;
  margin-bottom: 1.5rem;
  font-size: 1.2rem;
  color: #1976d2;
  text-align: center;
  font-weight: 600;
}

.scores-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(250px, 1fr));
  gap: 1rem;
  margin-bottom: 1.5rem;
}

.scores-warning {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  padding: 0.75rem;
  margin-bottom: 1rem;
  background-color: rgba(239, 68, 68, 0.1);
  border-radius: 8px;
  color: #ef4444;
  font-size: 0.9rem;
}

.scores-warning i {
  font-size: 1rem;
}

.save-button {
  width: 100%;
  margin-top: 0.5rem;
}

/* Loading & Error states */
.loading-container,
.error-container {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  padding: 3rem;
  gap: 1rem;
  text-align: center;
}

.retry-button {
  margin-top: 1rem;
}

/* Media Queries for Responsive Design */
@media (max-width: 768px) {
  .habit-journal {
    padding: 10px;
    margin: 10px auto;
  }

  .journal-header {
    flex-direction: column;
    align-items: flex-start;
    padding: 0.5rem;
  }

  .journal-header-right {
    width: 100%;
    justify-content: space-between;
    margin-top: 0.5rem;
  }

  .date-navigation {
    width: calc(100% - 3rem);
    flex: 1;
  }

  .date-picker {
    width: 100%;
  }

  .date-picker :deep(.p-inputtext) {
    font-size: 0.85rem;
  }

  .habit-cards {
    grid-template-columns: 1fr;
  }

  .scores-grid {
    grid-template-columns: 1fr;
  }

  .progress-section {
    padding: 0.75rem;
  }

  .habit-card {
    padding: 0.75rem;
    min-height: 50px;
  }

  .scores-container {
    padding: 1rem;
  }

  .progress-info {
    font-size: 0.9rem;
  }

  .impact-progress {
    height: 20px !important;
    font-size: 0.9rem;
  }

  .p-progressbar .p-progressbar-value {
    font-size: 0.9rem;
  }
}

/* Small mobile devices */
@media (max-width: 480px) {
  .habit-journal {
    padding: 5px;
  }

  .journal-title {
    font-size: 1.1rem;
  }

  .habit-category-label {
    font-size: 1rem;
  }

  .scores-title {
    font-size: 1.1rem;
    margin-bottom: 1rem;
  }

  .habit-impact {
    min-width: 35px;
    font-size: 0.8rem;
  }

  .loading-container,
  .error-container {
    padding: 2rem 1rem;
  }
}
</style>
