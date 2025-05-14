import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import { Habit } from '../types/habit'
import { loadHabits, loadUserHabits, saveUserHabits } from '../services/habits.service'

export const useHabitStore = defineStore('habit', () => {
  const allHabits = ref<Habit[]>([])
  const userHabitIds = ref<string[]>([])
  const isLoading = ref(false)
  const isSaving = ref(false)
  const hasError = ref(false)
  const errorMessage = ref<string | null>(null)
  const hasAllHabitsLoaded = ref(false)
  const hasUserHabitsLoaded = ref(false)

  // Track ongoing API requests to prevent duplicates
  const pendingAllHabitsRequest = ref<Promise<Habit[]> | null>(null)
  const pendingUserHabitsRequest = ref<Promise<string[]> | null>(null)

  const userHabits = computed(() =>
    allHabits.value.filter((habit) => userHabitIds.value.includes(habit.id))
  )

  const userHabitsCount = computed(() => userHabitIds.value.length)

  const groupedHabits = computed(() => {
    const groups: Record<string, Habit[]> = {}
    for (const habit of allHabits.value) {
      const cat = habit.category || 'Без категории'
      if (!groups[cat]) groups[cat] = []
      groups[cat].push(habit)
    }
    return groups
  })

  async function loadAllHabits() {
    // If data is already loaded, return it immediately
    if (hasAllHabitsLoaded.value && allHabits.value.length > 0) {
      return allHabits.value
    }

    // If there's an ongoing request, reuse it
    if (pendingAllHabitsRequest.value) {
      return pendingAllHabitsRequest.value
    }

    try {
      isLoading.value = true
      hasError.value = false
      errorMessage.value = null

      // Create and store the promise
      const promise = loadHabits()
      pendingAllHabitsRequest.value = promise

      // Await the result
      allHabits.value = await promise
      hasAllHabitsLoaded.value = true
      return allHabits.value
    } catch (err: any) {
      console.error('Error loading habits:', err)
      hasError.value = true
      errorMessage.value = err.message || 'Failed to load habits'
      return []
    } finally {
      isLoading.value = false
      pendingAllHabitsRequest.value = null
    }
  }

  async function loadUserHabitIds() {
    // If data is already loaded, return it immediately
    if (hasUserHabitsLoaded.value && userHabitIds.value.length > 0) {
      return userHabitIds.value
    }

    // If there's an ongoing request, reuse it
    if (pendingUserHabitsRequest.value) {
      return pendingUserHabitsRequest.value
    }

    try {
      isLoading.value = true
      hasError.value = false
      errorMessage.value = null

      // Create and store the promise
      const promise = loadUserHabits()
      pendingUserHabitsRequest.value = promise

      // Await the result
      userHabitIds.value = await promise
      hasUserHabitsLoaded.value = true
      return userHabitIds.value
    } catch (err: any) {
      console.error('Error loading user habits:', err)
      hasError.value = true
      errorMessage.value = err.message || 'Failed to load user habits'
      return []
    } finally {
      isLoading.value = false
      pendingUserHabitsRequest.value = null
    }
  }

  async function saveUserHabitSelection(habitIds: string[]) {
    try {
      isSaving.value = true
      hasError.value = false
      errorMessage.value = null
      await saveUserHabits(habitIds)
      userHabitIds.value = habitIds
      hasUserHabitsLoaded.value = true
      return true
    } catch (err: any) {
      console.error('Error saving user habits:', err)
      hasError.value = true
      errorMessage.value = err.message || 'Failed to save user habits'
      return false
    } finally {
      isSaving.value = false
    }
  }

  async function loadAllHabitsData() {
    // Only load what's not already loaded to prevent duplicate calls
    const promises: Promise<any>[] = []

    if (!hasAllHabitsLoaded.value && !pendingAllHabitsRequest.value) {
      promises.push(loadAllHabits())
    }

    if (!hasUserHabitsLoaded.value && !pendingUserHabitsRequest.value) {
      promises.push(loadUserHabitIds())
    }

    if (promises.length > 0) {
      await Promise.all(promises)
    }
  }

  return {
    allHabits,
    userHabitIds,
    userHabits,
    userHabitsCount,
    groupedHabits,
    isLoading,
    isSaving,
    hasError,
    errorMessage,
    hasAllHabitsLoaded,
    hasUserHabitsLoaded,
    loadAllHabits,
    loadUserHabitIds,
    saveUserHabitSelection,
    loadAllHabitsData,
  }
})
