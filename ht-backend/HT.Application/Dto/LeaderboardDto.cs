namespace HT.Application.Dto;

public record LeaderboardDto(List<LeaderboardUserDto> Users, LeaderboardUserDto CurrentUser);