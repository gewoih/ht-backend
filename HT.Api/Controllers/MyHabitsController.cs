using HT.Application.Common.Interfaces;
using HT.Application.Habits.Interfaces;
using HT.Domain.UserHabits;
using HT.Domain.Users;
using Microsoft.AspNetCore.Mvc;

namespace HT.Api.Controllers;

[Route("api/me/habits")]
[ApiController]
public class MyHabitsController(
    ICurrentUserService currentUserService,
    IUserRepository userRepository,
    IUserHabitRepository userHabitRepository,
    IUserHabitService userHabitService) : ControllerBase
{
    [HttpPut]
    public async Task<IActionResult> Put([FromBody] List<Guid> habitIds, CancellationToken cancellationToken)
    {
        var currentUserId = currentUserService.GetId();
        if (!await userRepository.ExistsAsync(currentUserId, cancellationToken))
            throw new Exception("User does not exist");

        await userHabitRepository.ReplaceAsync(currentUserId, habitIds, cancellationToken);
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        return Ok(await userHabitService.GetAsync(cancellationToken));
    }
}