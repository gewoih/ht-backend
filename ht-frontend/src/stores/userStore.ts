import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import { User, SubscriptionType, SubscriptionLimits } from '../types/user'
import { getCurrentUser, getSubscriptionLimits } from '../services/user.service'
import { useHabitStore } from './habitStore'

export const useUserStore = defineStore('user', () => {
  const user = ref<User | null>(null)
  const isLoading = ref(false)
  const hasError = ref(false)
  const errorMessage = ref<string | null>(null)

  const habitStore = useHabitStore()

  const isPremium = computed(() => user.value?.subscription.type === SubscriptionType.PREMIUM)

  const subscriptionLimits = computed<SubscriptionLimits>(() =>
    user.value ? getSubscriptionLimits(user.value.subscription.type) : { maxHabits: 0 }
  )

  const remainingHabitSlots = computed(
    () => subscriptionLimits.value.maxHabits - habitStore.userHabitsCount
  )

  const canAddMoreHabits = computed(
    () => isPremium.value || habitStore.userHabitsCount < subscriptionLimits.value.maxHabits
  )

  async function loadUserProfile() {
    try {
      isLoading.value = true
      hasError.value = false
      errorMessage.value = null
      user.value = await getCurrentUser()

      // Also load habits count
      await habitStore.loadUserHabitIds()
    } catch (err: any) {
      console.error('Error loading user profile:', err)
      hasError.value = true
      errorMessage.value = err.message || 'Failed to load user profile'
    } finally {
      isLoading.value = false
    }
  }

  return {
    user,
    isLoading,
    hasError,
    errorMessage,
    isPremium,
    subscriptionLimits,
    remainingHabitSlots,
    canAddMoreHabits,
    loadUserProfile,
  }
})
