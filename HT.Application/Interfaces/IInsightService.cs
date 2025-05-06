using HT.Application.Dto;

namespace HT.Application.Interfaces;

public interface IInsightService
{
    Task<List<InsightDto>> GetInsightsAsync(CancellationToken cancellationToken = default);
}