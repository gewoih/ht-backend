import { DailyScore } from './daily-score'

export interface AnalyticsEntry {
  date: string
  score: DailyScore
}

export type AnalyticsData = AnalyticsEntry[]
