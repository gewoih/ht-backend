using HT.Api.Features.Base;
using HT.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HT.Api.Features.Habits;

public class GetMyHabits : IEndpointFeature
{
    public RouteHandlerBuilder Map(RouteGroupBuilder group) =>
        group.MapGet("/me/habits", HandleAsync)
            .RequireAuthorization();

    private static async Task<IResult> HandleAsync(
        [FromServices] IUserHabitService service,
        CancellationToken ct)
    {
        var ids = await service.GetIdsAsync(ct);
        return Results.Ok(ids);
    }
}