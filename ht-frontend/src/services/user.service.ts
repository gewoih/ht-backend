import http from './http.service'
import { API_ENDPOINTS } from '../config/api'
import { User, SubscriptionType, SubscriptionLimits } from '../types/user'

// Get current user profile
export async function getCurrentUser(): Promise<User> {
  const response = await http.get(API_ENDPOINTS.user.profile)
  return response.data
}

// Get subscription limits based on subscription type
export function getSubscriptionLimits(subscriptionType: SubscriptionType): SubscriptionLimits {
  const limits: Record<SubscriptionType, SubscriptionLimits> = {
    [SubscriptionType.FREE]: {
      maxHabits: 10,
    },
    [SubscriptionType.PREMIUM]: {
      maxHabits: Infinity,
    },
  }

  return limits[subscriptionType]
}
