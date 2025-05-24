using HT.Api.Features.Base;
using HT.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HT.Api.Features.Profile;

internal sealed class GetProfile : IEndpointFeature
{
    public RouteHandlerBuilder Map(RouteGroupBuilder group) => group.MapGet("/me/profile", GetUserProfileAsync);

    private static async Task<IResult> GetUserProfileAsync(
        [FromServices] ICurrentUserService currentUserService,
        CancellationToken ct)
    {
        var currentUserProfile = await currentUserService.GetUserProfileAsync(ct);
        return currentUserProfile == null ? Results.NotFound() : Results.Ok(currentUserProfile);
    }
}