using HT.Api.Extensions;
using HT.Api.Features.Base;
using HT.Core.Infrastructure.Persistence;
using HT.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HT.Api.Features.Auth;

public class Logout : IEndpointFeature
{
    public RouteHandlerBuilder Map(RouteGroupBuilder group) => group.MapPost("/auth/logout", HandleAsync);

    private static async Task<IResult> HandleAsync(
        [FromServices] IAuthService auth,
        HttpRequest req,
        HttpResponse rsp,
        CancellationToken ct)
    {
        var token = req.Cookies[AuthService.RefreshTokenCookieName];
        var ok = await auth.LogoutAsync(token, ct);
        if (!ok) return Results.Unauthorized();

        rsp.Cookies.Delete(AuthService.RefreshTokenCookieName, CookieOptionsExtensions.Refresh());
        return Results.Ok();
    }
}