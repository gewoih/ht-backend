using HT.Api.Features.Base;
using HT.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HT.Api.Features.Habits;

internal sealed class GetAllHabits : IEndpointFeature
{
    public RouteHandlerBuilder Map(RouteGroupBuilder group) => group.MapGet("/habits", HandleAsync);

    private static async Task<IResult> HandleAsync(
        [FromServices] IHabitService service,
        CancellationToken ct)
    {
        var habits = await service.GetAsync(ct);
        return Results.Ok(habits);
    }
}