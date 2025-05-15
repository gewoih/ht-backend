using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HT.Domain.Entities.Identity;
using Microsoft.IdentityModel.Tokens;

namespace HT.Infrastructure.Persistence;

public static class JwtService
{
    public static string GenerateJwtToken(User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim("subscription", user.CurrentSubscription.Type.ToString()),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AuthService.JwtSecretKey));
        var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: AuthService.ValidIssuer,
            audience: AuthService.ValidAudience,
            claims: claims,
            expires: DateTime.UtcNow.AddDays(7),
            signingCredentials: signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}