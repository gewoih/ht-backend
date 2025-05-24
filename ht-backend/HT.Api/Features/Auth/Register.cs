using HT.Api.Features.Base;
using HT.Core.Dto.Requests;
using HT.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HT.Api.Features.Auth;

public class Register : IEndpointFeature
{
    public RouteHandlerBuilder Map(RouteGroupBuilder group) => group.MapPost("/auth/register", HandleAsync);

    private static async Task<IResult> HandleAsync(
        [FromBody] RegisterRequest request,
        [FromServices] IAuthService authService,
        CancellationToken ct)
    {
        var ok = await authService.RegisterAsync(request, ct);
        return ok ? Results.Ok() : Results.BadRequest();
    }
}