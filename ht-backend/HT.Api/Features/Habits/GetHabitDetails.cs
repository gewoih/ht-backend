using HT.Api.Features.Base;
using HT.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HT.Api.Features.Habits;

public class GetHabitDetails : IEndpointFeature
{
    public RouteHandlerBuilder Map(RouteGroupBuilder group) => group.MapGet("/{id:guid}/details", HandleAsync);

    private static async Task<IResult> HandleAsync(
        [FromRoute] Guid id,
        [FromServices] IHabitService service,
        CancellationToken ct)
    {
        var habit = await service.GetDetailsAsync(id, ct);
        return habit is null ? Results.NotFound() : Results.Ok(habit);
    }
}