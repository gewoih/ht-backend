using HT.Core.Dto;
using HT.Core.Dto.Requests;

namespace HT.Core.Interfaces;

public interface IAuthService
{
    Task<bool> RegisterAsync(RegisterRequest request, CancellationToken ct = default);
    Task<TokenPairDto?> SignInAsync(SignInRequest request, CancellationToken ct = default);
    Task<TokenPairDto?> RefreshAsync(string? refreshToken, CancellationToken ct = default);
    Task<bool> LogoutAsync(string? refreshToken, CancellationToken ct = default);
    Task<bool> SendEmailConfirmation(string email, CancellationToken ct = default);
    Task<bool> ConfirmEmailAsync(string email, int code, CancellationToken ct = default);
}