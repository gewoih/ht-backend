using HT.Api.Features.Base;
using HT.Core.Dto.Requests;
using HT.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HT.Api.Features.Journal;

internal sealed class UpdateMyJournal : IEndpointFeature
{
    public RouteHandlerBuilder Map(RouteGroupBuilder group) =>
        group.MapPut("/me/journal", HandleAsync)
            .RequireAuthorization();

    private static async Task<IResult> HandleAsync(
        [FromBody] CreateJournalLogRequest request,
        [FromServices] IUserJournalService service,
        CancellationToken ct)
    {
        await service.CreateAsync(request, ct);
        return Results.NoContent();
    }
}