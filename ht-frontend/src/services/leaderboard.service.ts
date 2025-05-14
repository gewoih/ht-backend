import { API_ENDPOINTS } from '../config/api'
import http from './http.service'
import type { Leaderboard } from '../types/leaderboard'

export const leaderboardService = {
  async getLeaderboard(fromDate: string, toDate: string): Promise<Leaderboard> {
    try {
      const response = await http.get(API_ENDPOINTS.leaderboard, {
        params: { fromDate, toDate },
      })
      return response.data
    } catch (error) {
      console.error('Error fetching leaderboard data:', error)
      throw error
    }
  },
}
