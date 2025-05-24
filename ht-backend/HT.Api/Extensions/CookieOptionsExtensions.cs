namespace HT.Api.Extensions;

public static class CookieOptionsExtensions
{
    public static CookieOptions Refresh() => new()
    {
        HttpOnly = true,
        Secure = true,
        SameSite = SameSiteMode.Strict,
        Expires = DateTimeOffset.UtcNow.AddDays(7),
        Path = "/api/auth"
    };
}