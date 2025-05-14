import { API_ENDPOINTS } from '../config/api'
import { JournalEntry } from '../types/journal-entry'
import http from './http.service'

export const loadJournalEntry = async (date: string): Promise<JournalEntry | null> => {
  try {
    const response = await http.get(`${API_ENDPOINTS.journal}?date=${date}`)
    return response.data
  } catch (error: any) {
    if (error.response?.status === 404) {
      return null
    }
    console.error('Ошибка при загрузке записи журнала:', error)
    throw error
  }
}

export const updateJournalEntry = async (data: JournalEntry): Promise<void> => {
  try {
    await http.put(API_ENDPOINTS.journal, data)
  } catch (error) {
    console.error('Ошибка при обновлении:', error)
    throw error
  }
}
