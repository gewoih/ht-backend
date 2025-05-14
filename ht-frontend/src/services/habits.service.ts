import http from './http.service'
import { API_ENDPOINTS } from '../config/api'
import { Habit } from '../types/habit'
import { HabitDetails } from '../types/habit-details'

// Загрузка всех привычек
export const loadHabits = async (): Promise<Habit[]> => {
  try {
    const response = await http.get(API_ENDPOINTS.habits)
    const data = response.data
    return data.map((habit: any) => ({
      ...habit,
      completed: false,
    }))
  } catch (error) {
    console.error('Ошибка при загрузке привычек:', error)
    throw error
  }
}

// Загрузка привычек пользователя
export const loadUserHabits = async (): Promise<string[]> => {
  try {
    const response = await http.get(API_ENDPOINTS.userHabits)
    return response.data
  } catch (error) {
    console.error('Ошибка при загрузке привычек:', error)
    throw error
  }
}

// Загрузка деталей привычки
export const loadHabitDetails = async (id: string): Promise<HabitDetails> => {
  try {
    const response = await http.get(`${API_ENDPOINTS.habits}/${id}/details`)
    return response.data
  } catch (error) {
    console.error('Ошибка загрузки деталей привычки:', error)
    throw error
  }
}

// Сохранение привычек пользователя
export const saveUserHabits = async (habitIds: string[]): Promise<void> => {
  try {
    await http.put(API_ENDPOINTS.userHabits, habitIds)
  } catch (error) {
    console.error('Ошибка при сохранении привычек:', error)
    throw error
  }
}
