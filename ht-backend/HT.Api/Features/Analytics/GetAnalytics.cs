using HT.Api.Features.Base;
using HT.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HT.Api.Features.Analytics;

internal sealed class GetAnalytics : IEndpointFeature
{
    public RouteHandlerBuilder Map(RouteGroupBuilder group) => group.MapGet("/me/analytics", HandleAsync);

    private static async Task<IResult> HandleAsync(
        [FromQuery] DateOnly fromDate,
        [FromQuery] DateOnly toDate,
        [FromServices] IUserAnalyticsService userAnalyticsService,
        CancellationToken ct) =>
        Results.Ok(await userAnalyticsService.GetDateScores(fromDate, toDate, ct));
}