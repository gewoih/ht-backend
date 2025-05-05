using HT.Application.Habits.Queries.GetAllHabits;
using HT.Application.Habits.Queries.GetHabitDetails;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HT.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HabitsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllHabits(CancellationToken cancellationToken)
    {
        return Ok(await mediator.Send(new GetAllHabitsQuery(), cancellationToken));
    }

    [HttpGet("{id:guid}/details")]
    public async Task<IActionResult> GetDetails([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        return Ok(await mediator.Send(new GetHabitDetailsQuery(id), cancellationToken));
    }
}