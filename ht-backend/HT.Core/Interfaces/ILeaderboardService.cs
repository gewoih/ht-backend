using HT.Core.Dto;

namespace HT.Core.Interfaces;

public interface ILeaderboardService
{
    Task<LeaderboardDto> GetAsync(DateOnly fromDate, DateOnly toDate, CancellationToken ct = default);
}