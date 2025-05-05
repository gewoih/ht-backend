using HT.Application.Insights.Dto;
using HT.Application.Insights.Interfaces;
using MediatR;

namespace HT.Application.Insights.Queries;

public record GetInsightsQuery : IRequest<List<InsightDto>>;

public class GetInsightsQueryHandler(IInsightService insightService) : IRequestHandler<GetInsightsQuery, List<InsightDto>>
{
    public async Task<List<InsightDto>> Handle(GetInsightsQuery request, CancellationToken cancellationToken)
    {
        return await insightService.GetInsightsAsync(cancellationToken);
    }
}