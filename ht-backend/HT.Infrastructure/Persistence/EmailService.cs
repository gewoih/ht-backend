using System.Security.Cryptography;
using HT.Application.Interfaces;
using HT.Domain.Entities.Identity;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;
using MimeKit;

namespace HT.Infrastructure.Persistence;

public class EmailService(UserManager<User> userManager, HtContext context) : IEmailService
{
    public async Task SendEmailAsync(string toEmail, string subject, string body, CancellationToken ct = default)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Track-me", "no-reply@track-me.ru"));
        message.To.Add(MailboxAddress.Parse(toEmail));
        message.Subject = subject;

        message.Body = new TextPart("plain")
        {
            Text = body
        };

        using var client = new SmtpClient();

        await client.ConnectAsync("smtp.msndr.net", 25, MailKit.Security.SecureSocketOptions.StartTls, ct);
        await client.AuthenticateAsync("nranenko@bk.ru", "75a5978c92dfc9b220b50c922b806c49", ct);
        await client.SendAsync(message, ct);
        await client.DisconnectAsync(true, ct);
    }

    public async Task<int> CreateEmailConfirmationCodeAsync(User user, CancellationToken ct = default)
    {
        var emailConfirmationToken = await userManager.GenerateEmailConfirmationTokenAsync(user);
        var code = RandomNumberGenerator.GetInt32(100_000, 999_999);
        var emailConfirmationCode = new EmailConfirmationCode
        {
            UserId = user.Id,
            ExpiresAt = DateTime.UtcNow.AddMinutes(5),
            Token = emailConfirmationToken,
            Code = code
        };

        await context.EmailConfirmationCodes.AddAsync(emailConfirmationCode, ct);
        await context.SaveChangesAsync(ct);

        return code;
    }
}