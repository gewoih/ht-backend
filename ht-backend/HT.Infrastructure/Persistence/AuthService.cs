using HT.Application.Dto.Requests;
using HT.Application.Interfaces;
using HT.Domain.Entities;
using HT.Domain.Entities.Identity;
using HT.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HT.Infrastructure.Persistence;

public class AuthService(UserManager<User> userManager, SignInManager<User> signInManager)
    : IAuthService
{
    public const string JwtSecretKey =
        "d6e9d94a5e3ac16ae94e0d4059198fba99faf83776fbdd839a1821cbf6bdefce9309652979ef97589c736f074b51362f91d77b425366978e7eb50dff551951f0445eab8856e164af3bbf4e39f1bc07d4ae22df788975fa28d9bd31403fad4a767bc20002fbf352fa9718ff2ec80b719dc98c3998f911610cf063166db5d43c8ebb2a40561b9b534197ac542f55d5c062839d6517018cd7a78e5e3f810d5a25e8e786d4557e6bfef63f041855338168a85eaedbf019c625f7a439bc303156040e9689c3158a7f49e4c7966ed2e21b0fbf12320e6b5fc419cccc9bec3febe0803cbb33083215d3ad7981eb20c59182e0d3154638041fb6a7577ca3e17de6bc300f";

    public const string ValidIssuer = "TrackMe.Api";
    public const string ValidAudience = "TrackMe.Spa";

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
        return !result.Succeeded ? string.Empty : JwtService.GenerateJwtToken(user);
    }
}