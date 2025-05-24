namespace HT.Core.Dto;

public record LeaderboardUserDto(Guid UserId, string Username, int Score, int Rank);