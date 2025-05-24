using HT.Core.Dto;

namespace HT.Core.Interfaces;

public interface IUserAnalyticsService
{
    Task<List<DateScoreDto>> GetDateScores(DateOnly fromDate, DateOnly toDate, CancellationToken ct = default);
}