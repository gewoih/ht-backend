using System.Security.Claims;
using HT.Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace HT.Infrastructure.Auth;

public sealed class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    public Guid GetId()
    {
        var user = httpContextAccessor.HttpContext?.User;
        var userIdClaim = user?.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
            throw new UnauthorizedAccessException("User ID claim not found");
        
        return Guid.TryParse(userIdClaim.Value, out var userId)
            ? userId
            : throw new UnauthorizedAccessException("User ID is missing or invalid.");
    }
}