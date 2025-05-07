using HT.Application.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HT.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpGet("login")]
    public IActionResult Login(string redirectUrl)
    {
        var returnUrl = Url.Action(nameof(GoogleCallback), "Auth", new { RedirectUrl = redirectUrl });
        var properties = new AuthenticationProperties { RedirectUri = returnUrl };
        return Challenge(properties, GoogleDefaults.AuthenticationScheme);
    }
    
    [HttpGet("google-callback")]
    public async Task<IActionResult> GoogleCallback(string redirectUrl, CancellationToken cancellationToken)
    {
        var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        if (!result.Succeeded || result.Principal == null)
            return Unauthorized();
        
        var token = await authService.SignInAsync(result.Principal, cancellationToken);
        if (string.IsNullOrEmpty(token))
            return Unauthorized();
        
        return Redirect($"{redirectUrl}?token={token}");
    }
}