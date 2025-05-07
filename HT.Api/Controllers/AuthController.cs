using HT.Application.Dto;
using HT.Application.Dto.Requests;
using HT.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HT.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var isRegistered = await authService.RegisterAsync(request);
        if (!isRegistered)
            return BadRequest();
        
        return Ok();
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] SignInRequest request)
    {
        var token = await authService.SignInAsync(request);
        if (string.IsNullOrEmpty(token))
            return Unauthorized();
        
        return Ok(token);
    }
}