using HT.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HT.Api.Controllers;

[Route("api/me/profile")]
[ApiController]
[Authorize]
public class MyProfileController(ICurrentUserService currentUserService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var currentUserProfile = await currentUserService.GetUserProfileAsync(cancellationToken);
        if (currentUserProfile == null)
            return NotFound();
        
        return Ok(currentUserProfile);
    }
}