import { DailyScore } from './daily-score'

export interface JournalEntry {
  date: string
  dailyScore: DailyScore
  habitLogs: {
    habitId: string
    value: boolean
  }[]
}
