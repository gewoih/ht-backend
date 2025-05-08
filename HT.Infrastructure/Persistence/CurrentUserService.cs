using System.Security.Claims;
using HT.Application.Dto;
using HT.Application.Interfaces;
using HT.Application.Mappers;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace HT.Infrastructure.Persistence;

public sealed class CurrentUserService(IHttpContextAccessor httpContextAccessor, HtContext context)
    : ICurrentUserService
{
    public Guid GetUserId()
    {
        var user = httpContextAccessor.HttpContext?.User;
        var userIdClaim = user?.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
            throw new UnauthorizedAccessException("User ID claim not found");

        return Guid.TryParse(userIdClaim.Value, out var userId)
            ? userId
            : throw new UnauthorizedAccessException("User ID is missing or invalid.");
    }

    public async Task<UserProfile?> GetUserProfileAsync(CancellationToken cancellationToken = default)
    {
        var currentUserId = GetUserId();
        return await context.Users
            .Include(user => user.Subscriptions)
            .Where(user => user.Id == currentUserId)
            .Select(user => user.ToUserProfile())
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
    }
}