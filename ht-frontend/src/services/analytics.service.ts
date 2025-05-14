import { API_ENDPOINTS } from '../config/api'
import http from './http.service'
import type { AnalyticsData } from '../types/analytics'

export const analyticsService = {
  async getAnalytics(fromDate: string, toDate: string): Promise<AnalyticsData> {
    try {
      const response = await http.get(API_ENDPOINTS.user.analytics, {
        params: { fromDate, toDate },
      })
      return response.data
    } catch (error) {
      console.error('Error fetching analytics data:', error)
      throw error
    }
  },
}
