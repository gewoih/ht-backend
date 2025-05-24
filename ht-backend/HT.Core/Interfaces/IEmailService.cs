using HT.Core.Entities.Identity;

namespace HT.Core.Interfaces;

public interface IEmailService
{
    Task SendEmailAsync(string toEmail, string subject, string body, CancellationToken ct = default);
    Task<int> CreateEmailConfirmationCodeAsync(User user, CancellationToken ct = default);
}