using HT.Api.Extensions;
using HT.Api.Features.Base;
using HT.Core.Infrastructure.Persistence;
using HT.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HT.Api.Features.Auth;

internal sealed class Refresh : IEndpointFeature
{
    public RouteHandlerBuilder Map(RouteGroupBuilder group) => group.MapPost("/auth/refresh", HandleAsync);

    private static async Task<IResult> HandleAsync(
        [FromServices] IAuthService auth,
        HttpRequest req,
        HttpResponse rsp,
        CancellationToken ct)
    {
        var token = req.Cookies[AuthService.RefreshTokenCookieName];
        var pair = await auth.RefreshAsync(token, ct);
        if (pair is null) return Results.Unauthorized();

        rsp.Cookies.Append(AuthService.RefreshTokenCookieName, pair.RefreshToken, CookieOptionsExtensions.Refresh());
        return Results.Ok(new { pair.AccessToken });
    }
}