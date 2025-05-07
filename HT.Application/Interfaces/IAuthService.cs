using HT.Application.Dto;
using HT.Application.Dto.Requests;

namespace HT.Application.Interfaces;

public interface IAuthService
{
    Task<bool> RegisterAsync(RegisterRequest request);
    Task<string> SignInAsync(SignInRequest request);
}