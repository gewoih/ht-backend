using HT.Application.Dto;
using HT.Application.Dto.Requests;
using HT.Application.Interfaces;
using HT.Domain;
using HT.Domain.Entities;
using HT.Domain.Entities.Identity;
using HT.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HT.Infrastructure.Persistence;

public class AuthService(UserManager<User> userManager, SignInManager<User> signInManager, JwtService jwtService)
    : IAuthService
{
    public async Task<bool> RegisterAsync(RegisterRequest request)
    {
        var user = new User
        {
            UserName = request.Email, 
            Email = request.Email, 
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

        var result = await userManager.CreateAsync(user, request.Password);
        return result.Succeeded;
    }

    public async Task<string> SignInAsync(SignInRequest request)
    {
        var user = await userManager.Users
            .Include(user => user.Subscriptions)
            .FirstOrDefaultAsync(user => user.NormalizedUserName == request.Email.ToUpperInvariant()); 
        
        if (user == null)
            return string.Empty;
        
        var result = await signInManager.CheckPasswordSignInAsync(user, request.Password, false);
        return !result.Succeeded ? string.Empty : jwtService.GenerateJwtToken(user);
    }
}