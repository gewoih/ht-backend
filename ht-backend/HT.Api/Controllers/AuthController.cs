using HT.Application.Dto.Requests;
using HT.Application.Interfaces;
using HT.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace HT.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken ct)
    {
        var isRegistered = await authService.RegisterAsync(request, ct);
        if (!isRegistered)
            return BadRequest();

        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] SignInRequest request, CancellationToken ct)
    {
        var tokenPairDto = await authService.SignInAsync(request, ct);
        if (tokenPairDto == null)
            return Unauthorized();

        SetRefreshCookie(tokenPairDto.RefreshToken);

        return Ok(new { tokenPairDto.AccessToken });
    }

    [HttpPost("email-confirmation")]
    public async Task<IActionResult> SendEmailConfirmation([FromQuery] string email, CancellationToken ct)
    {
        var isSent = await authService.SendEmailConfirmation(email, ct);
        return Ok(new { isSent });
    }
    
    [HttpPost("confirm-email")]
    public async Task<IActionResult> ConfirmEmail([FromQuery] string email, [FromQuery] int code, CancellationToken ct)
    {
        var isConfirmed = await authService.ConfirmEmailAsync(email, code, ct);
        if (!isConfirmed)
            return Unauthorized();
        
        return Ok();
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout(CancellationToken ct)
    {
        var refreshToken = Request.Cookies[AuthService.RefreshTokenCookieName];
        var isLoggedOut = await authService.LogoutAsync(refreshToken, ct); 
        if (!isLoggedOut)
            return Unauthorized();
        
        DeleteRefreshCookie();
        return Ok();
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(CancellationToken ct)
    {
        var refreshToken = Request.Cookies[AuthService.RefreshTokenCookieName];
        var tokenPairDto = await authService.RefreshAsync(refreshToken, ct);
        if (tokenPairDto is null)
            return Unauthorized();

        SetRefreshCookie(tokenPairDto.RefreshToken);
        return Ok(new { tokenPairDto.AccessToken });
    }

    private void SetRefreshCookie(string refreshToken) =>
        Response.Cookies.Append(AuthService.RefreshTokenCookieName, refreshToken, GetRefreshCookieOptions());

    private void DeleteRefreshCookie() =>
        Response.Cookies.Delete(AuthService.RefreshTokenCookieName, GetRefreshCookieOptions());

    private static CookieOptions GetRefreshCookieOptions() => new()
    {
        HttpOnly = true,
        Secure = true,
        SameSite = SameSiteMode.Strict,
        Expires = DateTimeOffset.UtcNow.AddDays(7),
        Path = "/api/auth"
    };
}