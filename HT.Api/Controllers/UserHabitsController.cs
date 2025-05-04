using HT.Common.Services;
using Microsoft.AspNetCore.Mvc;

namespace HT.Api.Controllers;

[Route("api/user/habits")]
[ApiController]
public class UserHabitsController(UserHabitService userHabitService) : ControllerBase
{
    [HttpPut]
    public async Task<IActionResult> Put([FromBody] List<Guid> habitIds)
    {
        await userHabitService.UpdateAsync(habitIds);
        return Ok();
    }
    
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok(await userHabitService.GetAsync());
    }
}