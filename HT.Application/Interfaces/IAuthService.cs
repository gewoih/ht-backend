using System.Security.Claims;

namespace HT.Application.Interfaces;

public interface IAuthService
{
    Task<string> SignInAsync(ClaimsPrincipal claimsPrincipal,
        CancellationToken cancellationToken = default);
}