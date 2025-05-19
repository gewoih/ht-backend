using System.Security.Cryptography;
using HT.Application.Interfaces;
using HT.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace HT.Infrastructure.Persistence;

public class FakeEmailService(UserManager<User> userManager, HtContext context) : IEmailService
{
    public Task SendEmailAsync(string toEmail, string subject, string body, CancellationToken ct = default)
    {
        return Task.CompletedTask;
    }

    public async Task<int> CreateEmailConfirmationCodeAsync(User user, CancellationToken ct = default)
    {
        var emailConfirmationToken = await userManager.GenerateEmailConfirmationTokenAsync(user);
        const int code = 111_111;
        var emailConfirmationCode = new EmailConfirmationCode
        {
            UserId = user.Id,
            ExpiresAt = DateTime.UtcNow.AddHours(1),
            Token = emailConfirmationToken,
            Code = code
        };

        await context.EmailConfirmationCodes.AddAsync(emailConfirmationCode, ct);
        await context.SaveChangesAsync(ct);
        return code;
    }
}