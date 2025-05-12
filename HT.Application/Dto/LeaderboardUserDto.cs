namespace HT.Application.Dto;

public record LeaderboardUserDto(Guid UserId, string Username, int Score, int Rank);