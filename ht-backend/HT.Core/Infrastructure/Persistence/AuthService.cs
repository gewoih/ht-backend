using System.Security.Cryptography;
using System.Text;
using HT.Core.Dto;
using HT.Core.Dto.Requests;
using HT.Core.Entities;
using HT.Core.Entities.Identity;
using HT.Core.Enums;
using HT.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HT.Core.Infrastructure.Persistence;

public class AuthService(
    UserManager<User> userManager,
    SignInManager<User> signInManager,
    IEmailService emailService,
    HtContext context)
    : IAuthService
{
    public const string JwtSecretKey =
        "d6e9d94a5e3ac16ae94e0d4059198fba99faf83776fbdd839a1821cbf6bdefce9309652979ef97589c736f074b51362f91d77b425366978e7eb50dff551951f0445eab8856e164af3bbf4e39f1bc07d4ae22df788975fa28d9bd31403fad4a767bc20002fbf352fa9718ff2ec80b719dc98c3998f911610cf063166db5d43c8ebb2a40561b9b534197ac542f55d5c062839d6517018cd7a78e5e3f810d5a25e8e786d4557e6bfef63f041855338168a85eaedbf019c625f7a439bc303156040e9689c3158a7f49e4c7966ed2e21b0fbf12320e6b5fc419cccc9bec3febe0803cbb33083215d3ad7981eb20c59182e0d3154638041fb6a7577ca3e17de6bc300f";

    public const string ValidIssuer = "TrackMe.Api";
    public const string ValidAudience = "TrackMe.Spa";
    public const string RefreshTokenCookieName = "refresh_token";

    public async Task<bool> RegisterAsync(RegisterRequest request, CancellationToken ct = default)
    {
        var currentDateUtc = DateTime.UtcNow;
        var user = new User
        {
            UserName = request.Username,
            Email = request.Email,
            Subscriptions = new List<Subscription>
            {
                new()
                {
                    IsActive = true,
                    Type = SubscriptionType.Free,
                    StartDate = currentDateUtc,
                    EndDate = currentDateUtc.AddDays(30)
                }
            }
        };

        var result = await userManager.CreateAsync(user, request.Password);
        return result.Succeeded;
    }

    public async Task<TokenPairDto?> SignInAsync(SignInRequest request, CancellationToken ct = default)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user is null)
            return null;

        var result = await signInManager.CheckPasswordSignInAsync(user, request.Password, true);
        if (!result.Succeeded)
            return null;

        await context.Entry(user).Collection(x => x.Subscriptions).LoadAsync(ct);
        var accessToken = JwtService.GenerateJwtToken(user);
        if (string.IsNullOrEmpty(accessToken))
            return null;

        var (rawRefreshToken, refreshToken) = JwtService.CreateRefreshToken(user.Id);
        await context.RefreshTokens.AddAsync(refreshToken, ct);
        await context.SaveChangesAsync(ct);

        return new TokenPairDto(rawRefreshToken, accessToken);
    }

    public async Task<TokenPairDto?> RefreshAsync(string? refreshToken, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
            return null;

        var hash = SHA512.HashData(Encoding.UTF8.GetBytes(refreshToken));
        var refreshTokenEntity = await context.RefreshTokens
            .Include(token => token.User)
            .ThenInclude(user => user.Subscriptions)
            .SingleOrDefaultAsync(t => t.Hash == hash, cancellationToken: ct);

        if (refreshTokenEntity is not { RevokedAt: null } || refreshTokenEntity.ExpiresAt <= DateTime.UtcNow)
            return null;

        if (refreshTokenEntity.ReplacedByTokenId != null)
        {
            await RevokeDescendantsAsync(refreshTokenEntity);
            return null;
        }

        var accessToken = JwtService.GenerateJwtToken(refreshTokenEntity.User);
        var (newRawRefreshToken, newRefreshToken) = JwtService.CreateRefreshToken(refreshTokenEntity.UserId);

        refreshTokenEntity.RevokedAt = DateTime.UtcNow;
        refreshTokenEntity.ReplacedByTokenId = newRefreshToken.Id;

        await context.RefreshTokens.AddAsync(newRefreshToken, ct);
        await context.SaveChangesAsync(ct);

        return new TokenPairDto(newRawRefreshToken, accessToken);
    }

    public async Task<bool> LogoutAsync(string? refreshToken, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
            return false;

        var refreshTokenHash = SHA512.HashData(Encoding.UTF8.GetBytes(refreshToken));
        var refreshTokenEntity = await context.RefreshTokens
            .SingleOrDefaultAsync(x => x.Hash == refreshTokenHash, cancellationToken: ct);

        if (refreshTokenEntity != null)
            refreshTokenEntity.RevokedAt = DateTime.UtcNow;

        await context.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> SendEmailConfirmation(string email, CancellationToken ct = default)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is null)
            return false;
        
        var code = await emailService.CreateEmailConfirmationCodeAsync(user, ct);
        await emailService.SendEmailAsync(email, "Код подтверждения",
            $"Ваш код подтверждения в сервисе TrackMe: {code}", ct);
        
        return true;
    }

    public async Task<bool> ConfirmEmailAsync(string email, int code, CancellationToken ct = default)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user == null)
            return false;

        var token = await context.EmailConfirmationCodes
            .Where(emailConfirmationCode => emailConfirmationCode.UserId == user.Id &&
                                            emailConfirmationCode.ExpiresAt > DateTime.UtcNow &&
                                            emailConfirmationCode.Code == code)
            .Select(emailConfirmationCode => emailConfirmationCode.Token)
            .FirstOrDefaultAsync(cancellationToken: ct);

        if (string.IsNullOrWhiteSpace(token))
            return false;

        var result = await userManager.ConfirmEmailAsync(user, token);
        return result.Succeeded;
    }

    private async Task RevokeDescendantsAsync(RefreshToken token)
    {
        var stack = new Stack<RefreshToken>();
        stack.Push(token);

        while (stack.Count != 0)
        {
            var current = stack.Pop();
            var children = await context.RefreshTokens
                .Where(t => t.ReplacedByTokenId == current.Id && t.RevokedAt == null)
                .ToListAsync();

            foreach (var child in children)
            {
                child.RevokedAt = DateTime.UtcNow;
                stack.Push(child);
            }
        }
    }
}