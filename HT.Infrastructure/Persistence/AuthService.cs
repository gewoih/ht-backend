using System.Security.Claims;
using HT.Application.Interfaces;
using HT.Domain;
using HT.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HT.Infrastructure.Persistence;

public class AuthService(HtContext context, JwtService jwtService) : IAuthService
{
    public async Task<string> SignInAsync(ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken = default)
    {
        var userName = claimsPrincipal.FindFirst(ClaimTypes.Name)?.Value;
        var userEmail = claimsPrincipal.FindFirst(ClaimTypes.Email)?.Value;
        
        if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(userEmail))
            return string.Empty;
        
        var user = await context.Users
            .Include(user => user.Subscriptions)
            .FirstOrDefaultAsync(user => user.Email == userEmail, cancellationToken: cancellationToken);

        if (user == null)
        {
            user = new User
            {
                Name = userName,
                Email = userEmail,
                Subscriptions = new List<Subscription>
                {
                    new()
                    {
                        IsActive = true,
                        Type = SubscriptionType.Free,
                        StartDate = DateTime.UtcNow,
                        EndDate = DateTime.MaxValue
                    }
                }
            };
            
            await context.Users.AddAsync(user, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }

        var token = jwtService.GenerateJwtToken(user);
        return token;
    }
}