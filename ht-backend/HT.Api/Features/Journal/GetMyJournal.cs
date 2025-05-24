using HT.Api.Features.Base;
using HT.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HT.Api.Features.Journal;

internal sealed class GetMyJournal : IEndpointFeature
{
    public RouteHandlerBuilder Map(RouteGroupBuilder group) =>
        group.MapGet("/me/journal", HandleAsync)
            .RequireAuthorization();

    private static async Task<IResult> HandleAsync(
        [FromQuery] DateOnly date,
        [FromServices] IUserJournalService service,
        CancellationToken ct)
    {
        var journal = await service.GetAsync(date, ct);
        return journal is null ? Results.NotFound() : Results.Ok(journal);
    }
}