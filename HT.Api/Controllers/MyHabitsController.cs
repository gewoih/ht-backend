using HT.Application.Habits.Commands.ReplaceUserHabits;
using HT.Application.Habits.Queries.GetHabitIds;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HT.Api.Controllers;

[Route("api/me/habits")]
[ApiController]
public class MyHabitsController(IMediator mediator) : ControllerBase
{
    [HttpPut]
    public async Task<IActionResult> Put([FromBody] List<Guid> habitIds, CancellationToken cancellationToken)
    {
        return Ok(await mediator.Send(new ReplaceUserHabitsCommand(habitIds), cancellationToken));
    }

    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        return Ok(await mediator.Send(new GetAllHabitIdsQuery(), cancellationToken));
    }
}