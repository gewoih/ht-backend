using HT.Domain.Entities.Identity;

namespace HT.Application.Interfaces;

public interface IEmailService
{
    Task SendEmailAsync(string toEmail, string subject, string body, CancellationToken ct = default);
    Task<int> CreateEmailConfirmationCodeAsync(User user, CancellationToken ct = default);
}