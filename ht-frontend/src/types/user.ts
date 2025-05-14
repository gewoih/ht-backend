export interface User {
  email: string
  subscription: SubscriptionInfo
}

export interface SubscriptionInfo {
  type: SubscriptionType
  startDate: string
  endDate: string
}

export enum SubscriptionType {
  FREE = 0,
  PREMIUM = 1,
}

export interface SubscriptionLimits {
  maxHabits: number
}
