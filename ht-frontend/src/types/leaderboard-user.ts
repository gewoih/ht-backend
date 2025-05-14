export interface LeaderboardUser {
  id: string
  username: string
  score: number
  rank: number
  avatarUrl?: string
  isCurrentUser?: boolean
}
