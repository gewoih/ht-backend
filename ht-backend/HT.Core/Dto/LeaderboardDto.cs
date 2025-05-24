namespace HT.Core.Dto;

public record LeaderboardDto(List<LeaderboardUserDto> Users, LeaderboardUserDto CurrentUser);