using HT.Api.Features.Base;
using HT.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HT.Api.Features.Auth;

public class ConfirmEmail : IEndpointFeature
{
    public RouteHandlerBuilder Map(RouteGroupBuilder group) => group.MapPost("/auth/confirm-email", HandleAsync);

    private static async Task<IResult> HandleAsync(
        [FromQuery] string email,
        [FromQuery] int code,
        [FromServices] IAuthService auth,
        CancellationToken ct)
    {
        var ok = await auth.ConfirmEmailAsync(email, code, ct);
        return ok ? Results.Ok() : Results.Unauthorized();
    }
}