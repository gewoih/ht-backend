using HT.Api.Features.Base;
using HT.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HT.Api.Features.Habits;

public class UpdateMyHabits : IEndpointFeature
{
    public RouteHandlerBuilder Map(RouteGroupBuilder group) =>
        group.MapPut("/me/habits", HandleAsync)
            .RequireAuthorization();

    private static async Task<IResult> HandleAsync(
        [FromBody] List<Guid> habitIds,
        [FromServices] IUserHabitService service,
        CancellationToken ct)
    {
        await service.ReplaceAsync(habitIds, ct);
        return Results.Ok();
    }
}