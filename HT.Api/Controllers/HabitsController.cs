using HT.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HT.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HabitsController(IHabitService habitService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken) =>
        Ok(await habitService.GetAsync(cancellationToken));

    [HttpGet("{id:guid}/details")]
    public async Task<IActionResult> GetDetails([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var habitDetails = await habitService.GetDetailsAsync(id, cancellationToken);
        return habitDetails == null ? NotFound() : Ok(habitDetails);
    }
}