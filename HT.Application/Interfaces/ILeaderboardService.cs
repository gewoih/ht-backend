using HT.Application.Dto;

namespace HT.Application.Interfaces;

public interface ILeaderboardService
{
    Task<LeaderboardDto> GetAsync(DateOnly fromDate, DateOnly toDate);
}