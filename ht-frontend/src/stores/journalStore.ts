import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import { format, addDays } from 'date-fns'
import { Habit } from '../types/habit'
import { DailyScore } from '../types/daily-score'
import { loadJournalEntry, updateJournalEntry } from '../services/journal.service'
import { useHabitStore } from './habitStore'

function getTodayString() {
  return format(new Date(), 'yyyy-MM-dd')
}

export const useJournalStore = defineStore('journal', () => {
  const habitStore = useHabitStore()
  const selectedDate = ref(new Date())
  const habits = ref<Habit[]>([])
  const dailyScore = ref<DailyScore>({
    health: 0,
    energy: 0,
    mood: 0,
    sleep: 0,
    calmness: 0,
    satisfaction: 0,
  })
  const isLoading = ref(false)
  const isSaving = ref(false)
  const hasError = ref(false)
  const errorMessage = ref<string | null>(null)
  const isEditable = ref(false)

  // Track ongoing journal load operation
  const pendingLoadOperation = ref<Promise<void> | null>(null)

  const isToday = computed(() => {
    return format(selectedDate.value, 'yyyy-MM-dd') === getTodayString()
  })

  const hasIncompleteScores = computed(() => {
    return Object.values(dailyScore.value).some((score) => score === 0)
  })

  function goToPreviousDay() {
    const newDate = addDays(selectedDate.value, -1)
    if (newDate <= new Date()) {
      selectedDate.value = newDate
    }
  }

  function goToNextDay() {
    if (!isToday.value) {
      selectedDate.value = addDays(selectedDate.value, 1)
    }
  }

  async function loadData() {
    // If there's an ongoing load operation, wait for it
    if (pendingLoadOperation.value) {
      return pendingLoadOperation.value
    }

    // Create the load operation
    const operation = (async () => {
      try {
        isLoading.value = true
        hasError.value = false
        errorMessage.value = null

        // First load all necessary habit data
        await habitStore.loadAllHabitsData()

        // Get journal entry for selected date (if exists)
        const dateString = format(selectedDate.value, 'yyyy-MM-dd')
        const journalEntry = await loadJournalEntry(dateString)

        if (journalEntry) {
          // We have a journal entry for this date
          dailyScore.value = journalEntry.dailyScore

          // Create a map of completed habit IDs and their completion status
          const completedHabits = new Map()
          journalEntry.habitLogs.forEach((log) => {
            completedHabits.set(log.habitId, log.value)
          })

          // Get all user habits and mark them as completed based on journal data
          habits.value = habitStore.userHabits.map((habit) => ({
            ...habit,
            completed: completedHabits.has(habit.id) ? completedHabits.get(habit.id) : false,
          }))
        } else {
          // No journal entry for this date, show all user habits as uncompleted
          dailyScore.value = {
            health: 0,
            energy: 0,
            mood: 0,
            sleep: 0,
            calmness: 0,
            satisfaction: 0,
          }

          habits.value = habitStore.userHabits.map((habit) => ({
            ...habit,
            completed: false,
          }))
        }
      } catch (err) {
        console.error('Ошибка при загрузке данных:', err)
        hasError.value = true
        errorMessage.value = 'Не удалось загрузить данные. Пожалуйста, попробуйте позже.'
      } finally {
        isLoading.value = false
        pendingLoadOperation.value = null
      }
    })()

    pendingLoadOperation.value = operation
    return operation
  }

  async function saveJournal() {
    // Prevent saving if any score is 0
    if (hasIncompleteScores.value) {
      hasError.value = true
      errorMessage.value = 'Пожалуйста, заполните все оценки перед сохранением.'
      return
    }

    try {
      isSaving.value = true
      hasError.value = false
      errorMessage.value = null

      await updateJournalEntry({
        date: format(selectedDate.value, 'yyyy-MM-dd'),
        dailyScore: dailyScore.value,
        habitLogs: habits.value.map((habit) => ({
          habitId: habit.id,
          value: habit.completed,
        })),
      })
    } catch (err) {
      console.error('Ошибка при сохранении:', err)
      hasError.value = true
      errorMessage.value = 'Не удалось сохранить данные. Пожалуйста, попробуйте позже.'
    } finally {
      isSaving.value = false
    }
  }

  return {
    selectedDate,
    habits,
    dailyScore,
    isLoading,
    isSaving,
    hasError,
    errorMessage,
    isEditable,
    isToday,
    hasIncompleteScores,
    goToPreviousDay,
    goToNextDay,
    loadData,
    saveJournal,
  }
})
