import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import { User, SubscriptionType } from '../types/user'
import { getCurrentUser } from '../services/user.service'
import { useHabitStore } from './habitStore'

export const useUserStore = defineStore('user', () => {
  const user = ref<User | null>(null)
  const isLoading = ref(false)
  const hasError = ref(false)
  const errorMessage = ref<string | null>(null)

  // Subscription status
  const isActiveSubscription = computed(() => {
    if (!user.value) return false
    if (!user.value.subscription) return false
    const now = Date.now()
    return (
      user.value.subscription.endDate && now < new Date(user.value.subscription.endDate).getTime()
    )
  })

  const isSubscriptionExpired = computed(() => {
    if (!user.value) return false
    if (!user.value.subscription) return false
    const now = Date.now()
    return (
      user.value.subscription.endDate && now >= new Date(user.value.subscription.endDate).getTime()
    )
  })

  // Plan info: infer from endDate and type (no explicit plan field)
  const subscriptionType = computed(() => {
    if (!user.value) return null
    return user.value.subscription?.type || null // SubscriptionType
  })

  async function loadUserProfile() {
    try {
      isLoading.value = true
      hasError.value = false
      errorMessage.value = null
      user.value = await getCurrentUser()
      // Also load habits count (if needed elsewhere)
      const habitStore = useHabitStore()
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
    isActiveSubscription,
    isSubscriptionExpired,
    subscriptionType,
    loadUserProfile,
  }
})
