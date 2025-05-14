import { LeaderboardUser } from './leaderboard-user'

export interface Leaderboard {
  users: LeaderboardUser[]
  currentUser: LeaderboardUser | null
}
