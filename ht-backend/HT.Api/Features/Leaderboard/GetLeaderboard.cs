using HT.Api.Features.Base;
using HT.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HT.Api.Features.Leaderboard;

internal sealed class GetLeaderboard : IEndpointFeature
{
    public RouteHandlerBuilder Map(RouteGroupBuilder group) => group.MapGet("/leaderboard", HandleAsync);

    private static async Task<IResult> HandleAsync(
        [FromQuery] DateOnly fromDate,
        [FromQuery] DateOnly toDate,
        [FromServices] ILeaderboardService leaderboardService,
        CancellationToken ct) =>
        Results.Ok(await leaderboardService.GetAsync(fromDate, toDate, ct));

}