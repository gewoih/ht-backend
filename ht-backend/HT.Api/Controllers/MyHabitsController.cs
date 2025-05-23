using HT.Application.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HT.Api.Controllers;

[Route("api/me/habits")]
[ApiController]
[Authorize]
public class MyHabitsController(IUserHabitService userHabitService) : ControllerBase
{
    [HttpPut]
    public async Task<IActionResult> Put([FromBody] List<Guid> habitIds, CancellationToken cancellationToken)
    {
        await userHabitService.ReplaceAsync(habitIds, cancellationToken);
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken) =>
        Ok(await userHabitService.GetIdsAsync(cancellationToken));
}