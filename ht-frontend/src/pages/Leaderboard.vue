<template>
  <div class="leaderboard">
    <Card class="leaderboard-card">
      <template #title>
        <span class="leaderboard-title">
          <i class="pi pi-trophy title-icon" /> Топ пользователей
        </span>
      </template>
      <template #content>
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

        <!-- Top users section -->
        <div class="user-list">
          <div v-for="(user, index) in topUsersList" :key="user.id" :class="userRowClass(user)">
            <div class="rank-badge">
              <div v-if="user.rank === 1" class="medal gold-medal">
                <i class="pi pi-trophy" />
                <span class="medal-shine"></span>
              </div>
              <div v-else-if="user.rank === 2" class="medal silver-medal">
                <i class="pi pi-trophy" />
                <span class="medal-shine"></span>
              </div>
              <div v-else-if="user.rank === 3" class="medal bronze-medal">
                <i class="pi pi-trophy" />
                <span class="medal-shine"></span>
              </div>
              <span v-else-if="user.rank > 0" class="rank-num">#{{ user.rank }}</span>
              <div v-else class="unranked-icon">
                <i class="pi pi-minus-circle" />
              </div>
            </div>

            <Avatar
              :image="user.avatarUrl"
              :label="getUserInitial(user)"
              shape="circle"
              class="user-avatar"
              :class="{ 'top-avatar': user.rank <= 3 }"
              size="large"
            />
            <span class="name">{{ user.username }}</span>
            <span class="score"> <i class="pi pi-bolt score-icon" /> {{ user.score }} очков </span>
          </div>
        </div>

        <!-- Current user section (when not in top) -->
        <div v-if="isCurrentUserAppended && currentUser" class="current-user-section">
          <div class="section-header"><i class="pi pi-user section-icon"></i> Ваша позиция</div>

          <div :class="userRowClass(currentUser)" key="current-user">
            <div class="rank-badge">
              <span v-if="currentUser.rank > 0" class="rank-num">#{{ currentUser.rank }}</span>
              <div v-else class="unranked-icon">
                <i class="pi pi-minus-circle" />
              </div>
            </div>
            <Avatar
              :image="currentUser.avatarUrl"
              :label="getUserInitial(currentUser)"
              shape="circle"
              class="user-avatar"
              size="large"
            />
            <span class="name">{{ currentUser.username }}</span>
            <span class="score">
              <i class="pi pi-bolt score-icon" /> {{ currentUser.score }} очков
            </span>
          </div>
        </div>
      </template>
    </Card>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, watch } from 'vue'
import Card from 'primevue/card'
import Button from 'primevue/button'
import Avatar from 'primevue/avatar'
import { LeaderboardUser } from '../types/leaderboard-user'
import { leaderboardService } from '../services/leaderboard.service'
import { startOfDay, startOfWeek, startOfMonth, formatISO } from 'date-fns'

const periodOptions = [
  { label: 'День', value: 'day' },
  { label: 'Неделя', value: 'week' },
  { label: 'Месяц', value: 'month' },
]

const selectedPeriod = ref<'day' | 'week' | 'month'>('week')

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
}

watch(selectedPeriod, loadLeaderboard, { immediate: true })

function userRowClass(user: LeaderboardUser) {
  if (user.isCurrentUser) return 'user-row current'
  if (user.rank === 1) return 'user-row gold'
  if (user.rank === 2) return 'user-row silver'
  if (user.rank === 3) return 'user-row bronze'
  return 'user-row'
}

function badgeSeverity(rank: number) {
  if (rank === 1) return 'warning'
  if (rank === 2) return 'secondary'
  if (rank === 3) return 'help'
  return 'info'
}

function getUserInitial(user: LeaderboardUser): string {
  return user.username && user.username.length > 0 ? user.username[0].toUpperCase() : ''
}

function changePeriod(period: 'day' | 'week' | 'month') {
  selectedPeriod.value = period
}
</script>

<style scoped>
.leaderboard {
  max-width: 500px;
  margin: auto;
  padding: 1rem;
}
.leaderboard-card {
  border-radius: 1.5rem;
  box-shadow: 0 2px 16px rgba(0, 0, 0, 0.07);
  background: #fff;
}
.leaderboard-title {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  font-size: 1.5rem;
  font-weight: 700;
}
.title-icon {
  color: #ffd700;
  font-size: 1.7rem;
}
.period-selector {
  display: flex;
  gap: 8px;
  margin-bottom: 16px;
  justify-content: center;
}
.user-list {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
  margin-bottom: 1rem;
}
.user-row {
  display: flex;
  align-items: center;
  gap: 0.75rem;
  padding: 0.75rem 0.5rem;
  border-radius: 1rem;
  background: #f5f7fa;
  transition: all 0.3s ease;
  position: relative;
}
.user-row.gold {
  background: linear-gradient(90deg, rgba(255, 215, 0, 0.3) 0%, rgba(255, 215, 0, 0.1) 100%);
  border: 2px solid rgba(255, 215, 0, 0.7);
  box-shadow: 0 4px 12px rgba(255, 215, 0, 0.25);
  padding: 1rem 0.75rem;
}
.user-row.silver {
  background: linear-gradient(90deg, rgba(192, 192, 192, 0.3) 0%, rgba(192, 192, 192, 0.1) 100%);
  border: 2px solid rgba(192, 192, 192, 0.7);
  box-shadow: 0 4px 12px rgba(192, 192, 192, 0.25);
  padding: 0.9rem 0.65rem;
}
.user-row.bronze {
  background: linear-gradient(90deg, rgba(205, 127, 50, 0.3) 0%, rgba(205, 127, 50, 0.1) 100%);
  border: 2px solid rgba(205, 127, 50, 0.7);
  box-shadow: 0 4px 12px rgba(205, 127, 50, 0.25);
  padding: 0.85rem 0.6rem;
}
.user-row.current {
  border: 2px solid #007bff;
  background: #e7f0ff;
  box-shadow: 0 2px 8px rgba(0, 123, 255, 0.08);
}
.current-user-section {
  margin-top: 1.5rem;
  border-top: 1px solid rgba(0, 123, 255, 0.2);
  padding-top: 1rem;
}
.section-header {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  color: #007bff;
  font-weight: 600;
  font-size: 1rem;
  margin-bottom: 0.75rem;
}
.section-icon {
  color: #007bff;
}
.rank-badge {
  min-width: 36px;
  display: flex;
  justify-content: center;
  align-items: center;
}
.medal {
  width: 36px;
  height: 36px;
  border-radius: 50%;
  display: flex;
  justify-content: center;
  align-items: center;
  font-weight: bold;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2);
  position: relative;
  overflow: hidden;
}
.gold-medal {
  background: linear-gradient(135deg, #ffd700 0%, #ffcc00 50%, #ffd700 100%);
  color: #8a6e00;
  border: 2px solid #ffcc00;
}
.gold-medal::after {
  content: '';
  position: absolute;
  top: -50%;
  left: -50%;
  width: 200%;
  height: 200%;
  background: rgba(255, 255, 255, 0.3);
  transform: rotate(45deg);
}
.silver-medal {
  background: linear-gradient(135deg, #e0e0e0 0%, #c0c0c0 50%, #e0e0e0 100%);
  color: #5e5e5e;
  border: 2px solid #c0c0c0;
}
.silver-medal::after {
  content: '';
  position: absolute;
  top: -50%;
  left: -50%;
  width: 200%;
  height: 200%;
  background: rgba(255, 255, 255, 0.3);
  transform: rotate(45deg);
}
.bronze-medal {
  background: linear-gradient(135deg, #cd7f32 0%, #b87333 50%, #cd7f32 100%);
  color: #63400f;
  border: 2px solid #b87333;
}
.bronze-medal::after {
  content: '';
  position: absolute;
  top: -50%;
  left: -50%;
  width: 200%;
  height: 200%;
  background: rgba(255, 255, 255, 0.3);
  transform: rotate(45deg);
}
@keyframes shine {
  0% {
    transform: translateX(-100%) rotate(45deg);
  }
  50%,
  100% {
    transform: translateX(100%) rotate(45deg);
  }
}
.rank-num {
  font-weight: 600;
  color: #888;
}
.unranked-icon {
  color: #aaa;
  font-size: 1.2rem;
  display: flex;
  align-items: center;
  justify-content: center;
}
.user-avatar {
  border: 2px solid #fff;
  box-shadow: 0 1px 4px rgba(0, 0, 0, 0.04);
}
.user-row.gold .user-avatar,
.user-row.silver .user-avatar,
.user-row.bronze .user-avatar {
  border: 3px solid #fff;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.15);
}
.name {
  flex: 1;
  font-weight: 500;
  font-size: 1.1rem;
  color: #2b3674;
}
.user-row.gold .name {
  font-weight: 700;
  color: #000;
}
.score {
  display: flex;
  align-items: center;
  gap: 0.25rem;
  font-weight: 600;
  color: #007bff;
}
.user-row.gold .score {
  font-weight: 700;
  color: #ffa000;
}
.score-icon {
  color: #f7b500;
  font-size: 1.1rem;
}
.rank-label {
  font-size: 0.75rem;
  font-weight: 600;
  padding: 0.15rem 0.5rem;
  border-radius: 1rem;
  margin-right: -8px;
}
.rank-1 {
  background: rgba(255, 215, 0, 0.25);
  color: #8a6e00;
}
.rank-2 {
  background: rgba(192, 192, 192, 0.25);
  color: #5e5e5e;
}
.rank-3 {
  background: rgba(205, 127, 50, 0.25);
  color: #63400f;
}
.top-avatar {
  border: 3px solid #fff;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.15);
}
@media (max-width: 600px) {
  .leaderboard {
    padding: 0.5rem;
  }
  .leaderboard-title {
    font-size: 1.1rem;
  }
  .user-row {
    padding: 0.5rem 0.25rem;
    gap: 0.5rem;
  }
  .user-row.gold,
  .user-row.silver,
  .user-row.bronze {
    transform: none;
    padding: 0.6rem 0.4rem;
  }
  .user-avatar {
    width: 32px;
    height: 32px;
  }
  .medal {
    width: 30px;
    height: 30px;
    font-size: 0.8rem;
  }
  .rank-label {
    font-size: 0.65rem;
    padding: 0.1rem 0.4rem;
  }
}
</style>
