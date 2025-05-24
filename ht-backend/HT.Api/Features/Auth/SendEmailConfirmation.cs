using HT.Api.Features.Base;
using HT.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HT.Api.Features.Auth;

public class SendEmailConfirmation : IEndpointFeature
{
    public RouteHandlerBuilder Map(RouteGroupBuilder group) => group.MapPost("/auth/email-confirmation", HandleAsync);

    private static async Task<IResult> HandleAsync(
        [FromQuery] string email,
        [FromServices] IAuthService auth,
        CancellationToken ct)
    {
        var sent = await auth.SendEmailConfirmation(email, ct);
        return Results.Ok(new { sent });
    }
}