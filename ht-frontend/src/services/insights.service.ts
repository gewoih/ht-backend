import http from './http.service'
import { API_ENDPOINTS } from '../config/api'

export async function fetchInsights() {
  const res = await http.get(API_ENDPOINTS.insights)
  return res.data
}
