using HT.Common.Services;
using Microsoft.AspNetCore.Mvc;

namespace HT.Api.Controllers;

[Route("api/me/habits")]
[ApiController]
public class MyHabitsController(UserHabitService userHabitService) : ControllerBase
{
    [HttpPut]
    public async Task<IActionResult> Put([FromBody] List<Guid> habitIds)
    {
        await userHabitService.ReplaceAsync(habitIds);
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok(await userHabitService.GetIdsAsync());
    }
}