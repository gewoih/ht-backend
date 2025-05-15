using HT.Application.Dto;
using HT.Application.Dto.Requests;

namespace HT.Application.Interfaces;

public interface IAuthService
{
    Task<bool> RegisterAsync(RegisterRequest request);
    Task<TokenPairDto?> SignInAsync(SignInRequest request);
    Task<TokenPairDto?> RefreshAsync(string? refreshToken);
    Task<bool> LogoutAsync(string? refreshToken);
}