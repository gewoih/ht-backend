using HT.Application.Insights.Dto;

namespace HT.Application.Insights.Interfaces;

public interface IInsightService
{
    Task<List<InsightDto>> GetInsightsAsync(CancellationToken cancellationToken = default);
}