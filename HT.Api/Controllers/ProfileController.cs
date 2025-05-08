using HT.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HT.Api.Controllers;

[Route("api/me/[Controller]")]
[ApiController]
[Authorize]
public class ProfileController(ICurrentUserService currentUserService) : ControllerBase
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