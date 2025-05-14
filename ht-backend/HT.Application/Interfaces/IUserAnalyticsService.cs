using HT.Application.Dto;

namespace HT.Application.Interfaces;

public interface IUserAnalyticsService
{
    Task<List<DateScoreDto>> GetDateScores(DateOnly fromDate, DateOnly toDate, CancellationToken ct = default);
}