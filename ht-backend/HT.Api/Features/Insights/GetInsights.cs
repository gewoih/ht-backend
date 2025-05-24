using HT.Api.Features.Base;
using HT.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HT.Api.Features.Insights;

internal sealed class GetInsights : IEndpointFeature
{
    public RouteHandlerBuilder Map(RouteGroupBuilder group) =>
        group.MapGet("/me/insight", HandleAsync)
            .RequireAuthorization();

    private static async Task<IResult> HandleAsync(
        [FromServices] IInsightService service,
        CancellationToken ct)
    {
        var insights = await service.GetInsightsAsync(ct);
        return Results.Ok(insights);
    }
}