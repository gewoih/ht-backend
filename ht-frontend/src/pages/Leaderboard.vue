<template>
  <div class="leaderboard">
    <Card class="leaderboard-card">
      <template #title>
        <div class="leaderboard-header">
          <h2 class="leaderboard-title">
            <i class="pi pi-award title-icon"></i> Топ-10 пользователей
          </h2>
          <div class="period-selector">
            <Button
              v-for="option in periodOptions"
              :key="option.value"
              :label="option.label"
              :class="{ active: selectedPeriod === option.value }"
              :outlined="selectedPeriod !== option.value"
              :severity="selectedPeriod === option.value ? 'primary' : 'secondary'"
              @click="changePeriod(option.value as 'day' | 'week' | 'month')"
              size="small"
            />
          </div>
        </div>
      </template>

      <template #content>
        <div class="leaderboard-content">
          <!-- Loading state -->
          <div v-if="loading" class="loading-state">
            <ProgressSpinner />
            <p>Загрузка рейтинга...</p>
          </div>

          <!-- Empty state -->
          <div v-else-if="topUsersList.length === 0" class="empty-state">
            <i class="pi pi-users empty-icon"></i>
            <p>Пока нет пользователей в рейтинге</p>
          </div>

          <!-- User rankings -->
          <div v-else class="user-rankings">
            <!-- Users list -->
            <div class="user-list">
              <div
                v-for="user in topUsersList"
                :key="user.id"
                :class="[
                  'user-item',
                  getRankClass(user.rank),
                  { 'current-user': user.isCurrentUser },
                ]"
              >
                <div class="user-main">
                  <div class="rank-container">
                    <!-- Top 3 positions get trophy icons -->
                    <span v-if="user.rank === 1" class="trophy gold">
                      <i class="pi pi-trophy"></i>
                    </span>
                    <span v-else-if="user.rank === 2" class="trophy silver">
                      <i class="pi pi-trophy"></i>
                    </span>
                    <span v-else-if="user.rank === 3" class="trophy bronze">
                      <i class="pi pi-trophy"></i>
                    </span>
                    <!-- Other positions get rank numbers -->
                    <span v-else class="rank-number">#{{ user.rank }}</span>
                  </div>

                  <div class="user-info">
                    <Avatar
                      :image="user.avatarUrl"
                      :label="getUserInitial(user)"
                      shape="circle"
                      class="user-avatar"
                    />
                    <span class="username">{{ user.username }}</span>
                  </div>
                </div>

                <div class="score-container">
                  <span class="score">{{ user.score }} <i class="pi pi-bolt score-icon"></i></span>
                </div>
              </div>
            </div>

            <!-- Current user section (when not in top) -->
            <div v-if="isCurrentUserAppended && currentUser" class="current-user-section">
              <div class="section-header">
                <i class="pi pi-user section-icon"></i>
                <span>Ваша позиция</span>
              </div>

              <div class="user-item current-user">
                <div class="user-main">
                  <div class="rank-container">
                    <span v-if="currentUser.rank > 0" class="rank-number"
                      >#{{ currentUser.rank }}</span
                    >
                    <span v-else class="unranked">—</span>
                  </div>

                  <div class="user-info">
                    <Avatar
                      :image="currentUser.avatarUrl"
                      :label="getUserInitial(currentUser)"
                      shape="circle"
                      class="user-avatar"
                    />
                    <span class="username">{{ currentUser.username }}</span>
                  </div>
                </div>

                <div class="score-container">
                  <span class="score"
                    >{{ currentUser.score }} <i class="pi pi-bolt score-icon"></i
                  ></span>
                </div>
              </div>
            </div>
          </div>
        </div>
      </template>
    </Card>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, watch, onMounted } from 'vue'
import Card from 'primevue/card'
import Button from 'primevue/button'
import Avatar from 'primevue/avatar'
import ProgressSpinner from 'primevue/progressspinner'
import { LeaderboardUser } from '../types/leaderboard-user'
import { leaderboardService } from '../services/leaderboard.service'
import { startOfDay, startOfWeek, startOfMonth, formatISO } from 'date-fns'

const periodOptions = [
  { label: 'День', value: 'day' },
  { label: 'Неделя', value: 'week' },
  { label: 'Месяц', value: 'month' },
]

const selectedPeriod = ref<'day' | 'week' | 'month'>('week')
const loading = ref(true)
const topUsers = ref<LeaderboardUser[]>([])
const currentUser = ref<LeaderboardUser | null>(null)
const isCurrentUserAppended = ref(false)

// Only show actual top users (not the appended current user)
const topUsersList = computed(() => {
  if (isCurrentUserAppended.value && topUsers.value.length > 0) {
    return topUsers.value.slice(0, -1) // Remove the last item which is the appended current user
  }
  return topUsers.value
})

function getDateRange(period: 'day' | 'week' | 'month') {
  const now = startOfDay(new Date())
  let fromDate: Date
  switch (period) {
    case 'day':
      fromDate = now
      break
    case 'week':
      fromDate = startOfWeek(now, { weekStartsOn: 1 })
      break
    case 'month':
      fromDate = startOfMonth(now)
      break
    default:
      fromDate = now
  }
  return {
    fromDate: formatISO(fromDate, { representation: 'date' }),
    toDate: formatISO(now, { representation: 'date' }),
  }
}

async function loadLeaderboard() {
  loading.value = true

  try {
    const { fromDate, toDate } = getDateRange(selectedPeriod.value)
    const response = await leaderboardService.getLeaderboard(fromDate, toDate)

    // Define types for API response format
    interface ApiUser {
      userId: string
      username: string
      score: number
      rank: number
    }

    interface ApiResponse {
      users: ApiUser[]
      currentUser: ApiUser | null
    }

    // Explicitly cast the response
    const apiData = response as unknown as ApiResponse

    // Map API users to our LeaderboardUser format
    const mappedUsers = apiData.users.map((user) => ({
      id: user.userId,
      username: user.username,
      score: user.score,
      rank: user.rank,
      isCurrentUser: apiData.currentUser ? user.userId === apiData.currentUser.userId : false,
    }))

    if (apiData.currentUser) {
      const currentUserObj = {
        id: apiData.currentUser.userId,
        username: apiData.currentUser.username,
        score: apiData.currentUser.score,
        rank: apiData.currentUser.rank,
        isCurrentUser: true,
      }

      const isInTop = mappedUsers.some((u) => u.id === currentUserObj.id)
      isCurrentUserAppended.value = !isInTop

      if (!isInTop) {
        mappedUsers.push(currentUserObj)
      }

      topUsers.value = mappedUsers
      currentUser.value = currentUserObj
    } else {
      topUsers.value = mappedUsers
      currentUser.value = null
      isCurrentUserAppended.value = false
    }
  } catch (error) {
    console.error('Failed to load leaderboard:', error)
    topUsers.value = []
  } finally {
    loading.value = false
  }
}

function getRankClass(rank: number): string {
  if (rank === 1) return 'gold-rank'
  if (rank === 2) return 'silver-rank'
  if (rank === 3) return 'bronze-rank'
  return ''
}

function getUserInitial(user: LeaderboardUser): string {
  return user.username && user.username.length > 0 ? user.username[0].toUpperCase() : ''
}

function changePeriod(period: 'day' | 'week' | 'month') {
  selectedPeriod.value = period
}

watch(selectedPeriod, loadLeaderboard)

onMounted(() => {
  loadLeaderboard()
})
</script>

<style scoped>
.leaderboard {
  max-width: 700px;
  margin: 0 auto;
  padding: 1rem;
}

.leaderboard-card {
  border-radius: 16px;
  box-shadow: 0 4px 20px rgba(0, 0, 0, 0.08);
  background: #fff;
}

.leaderboard-header {
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

.leaderboard-title {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  font-size: 1rem;
  font-weight: 600;
  margin: 0;
  color: var(--primary-color, #4318ff);
}

.title-icon {
  color: var(--primary-color, #4318ff);
  font-size: 1.6rem;
}

.period-selector {
  display: flex;
  gap: 0.5rem;
  flex-wrap: wrap;
}

.leaderboard-content {
  padding: 0.5rem 0;
}

.loading-state,
.empty-state {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  min-height: 200px;
  text-align: center;
  color: var(--text-color-secondary, #64748b);
}

.empty-icon {
  font-size: 3rem;
  margin-bottom: 1rem;
  opacity: 0.5;
}

/* User list */
.user-list {
  display: flex;
  flex-direction: column;
  gap: 0.75rem;
}

.user-item {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 0.75rem;
  border-radius: 12px;
  background-color: #f8fafc;
  transition: transform 0.2s, box-shadow 0.2s;
}

.user-item:hover {
  transform: translateY(-2px);
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.05);
}

.user-main {
  display: flex;
  align-items: center;
  gap: 0.75rem;
  flex: 1;
  min-width: 0; /* Enable text truncation */
}

.gold-rank {
  background-color: rgba(255, 215, 0, 0.1);
  border-left: 4px solid #ffd700;
}

.silver-rank {
  background-color: rgba(192, 192, 192, 0.1);
  border-left: 4px solid #c0c0c0;
}

.bronze-rank {
  background-color: rgba(205, 127, 50, 0.1);
  border-left: 4px solid #cd7f32;
}

.current-user {
  background-color: rgba(67, 24, 255, 0.05);
  border-left: 4px solid var(--primary-color, #4318ff);
}

.rank-container {
  min-width: 40px;
  display: flex;
  justify-content: center;
}

.rank-number {
  font-weight: 600;
  font-size: 0.875rem;
  color: var(--text-color-secondary, #64748b);
}

.gold-rank .rank-number {
  color: #a67c00;
  font-weight: 700;
}

.silver-rank .rank-number {
  color: #666666;
  font-weight: 700;
}

.bronze-rank .rank-number {
  color: #7d4e20;
  font-weight: 700;
}

.user-info {
  display: flex;
  align-items: center;
  gap: 0.75rem;
  min-width: 0; /* Enable text truncation */
  flex: 1;
}

.user-avatar {
  flex-shrink: 0;
}

.username {
  font-weight: 500;
  font-size: 1rem;
  color: var(--text-color, #1e293b);
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
  max-width: 100%;
}

.score-container {
  display: flex;
  align-items: center;
  padding-left: 1rem;
}

.score {
  font-size: 1rem;
  color: var(--primary-color, #4318ff);
  font-weight: 600;
  display: flex;
  align-items: center;
  gap: 0.25rem;
}

.score-icon {
  color: #f7b500;
  font-size: 1rem;
}

/* Current user section */
.current-user-section {
  margin-top: 1.5rem;
  border-top: 1px solid rgba(67, 24, 255, 0.1);
  padding-top: 1rem;
}

.section-header {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  margin-bottom: 0.75rem;
  font-size: 0.9rem;
  font-weight: 600;
  color: var(--primary-color, #4318ff);
}

.unranked {
  color: var(--text-color-secondary, #64748b);
  font-weight: 600;
}

/* Responsive adjustments */
@media (min-width: 768px) {
  .leaderboard-header {
    flex-direction: row;
    justify-content: space-between;
    align-items: center;
  }

  .leaderboard-title {
    font-size: 1.75rem;
  }

  .title-icon {
    font-size: 1.8rem;
  }

  .user-item {
    padding: 0.9rem 1.25rem;
    gap: 1rem;
  }

  .user-main {
    gap: 1rem;
  }

  .user-avatar {
    width: 3rem !important;
    height: 3rem !important;
  }

  .username {
    font-size: 1.1rem;
  }

  .rank-container {
    min-width: 50px;
  }

  .rank-number {
    font-size: 1rem;
  }

  .score {
    font-size: 1.1rem;
  }

  .score-icon {
    font-size: 1.2rem;
  }

  .section-header {
    font-size: 1.05rem;
  }
}

@media (max-width: 480px) {
  .leaderboard {
    padding: 0.5rem;
  }

  .leaderboard-title {
    font-size: 1.25rem;
  }

  .user-item {
    padding: 0.6rem;
  }

  .rank-container {
    min-width: 30px;
  }

  .user-main {
    gap: 0.5rem;
  }

  .username {
    font-size: 0.9rem;
  }

  .score {
    font-size: 0.85rem;
  }
}

.trophy {
  display: flex;
  justify-content: center;
  align-items: center;
  width: 2rem;
  height: 2rem;
  border-radius: 50%;
  box-shadow: 0 2px 6px rgba(0, 0, 0, 0.1);
}

.trophy i {
  font-size: 1rem;
}

.trophy.gold {
  background: linear-gradient(135deg, #ffd700 0%, #ffc800 100%);
  color: #a67c00;
}

.trophy.silver {
  background: linear-gradient(135deg, #e0e0e0 0%, #c0c0c0 100%);
  color: #666666;
}

.trophy.bronze {
  background: linear-gradient(135deg, #cd7f32 0%, #b87333 100%);
  color: #7d4e20;
}

@media (min-width: 768px) {
  .trophy {
    width: 2.25rem;
    height: 2.25rem;
  }

  .trophy i {
    font-size: 1.2rem;
  }
}
</style>
