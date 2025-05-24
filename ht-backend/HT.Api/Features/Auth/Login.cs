using HT.Api.Extensions;
using HT.Api.Features.Base;
using HT.Core.Dto.Requests;
using HT.Core.Infrastructure.Persistence;
using HT.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HT.Api.Features.Auth;

public class Login : IEndpointFeature
{
    public RouteHandlerBuilder Map(RouteGroupBuilder group) => group.MapPost("/auth/login", HandleAsync);

    private static async Task<IResult> HandleAsync(
        [FromBody] SignInRequest request,
        [FromServices] IAuthService auth,
        HttpResponse response,
        CancellationToken ct)
    {
        var pair = await auth.SignInAsync(request, ct);
        if (pair is null) return Results.Unauthorized();

        SetRefreshCookie(response, pair.RefreshToken);
        return Results.Ok(new { pair.AccessToken });
    }

    private static void SetRefreshCookie(HttpResponse response, string token) =>
        response.Cookies.Append(AuthService.RefreshTokenCookieName, token, CookieOptionsExtensions.Refresh());
}