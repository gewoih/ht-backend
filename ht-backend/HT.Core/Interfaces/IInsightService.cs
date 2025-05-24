using HT.Core.Dto;

namespace HT.Core.Interfaces;

public interface IInsightService
{
    Task<List<InsightDto>> GetInsightsAsync(CancellationToken cancellationToken = default);
}