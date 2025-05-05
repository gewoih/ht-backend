using HT.Application.Journal.Commands;
using HT.Application.Journal.Dto;
using HT.Application.Journal.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HT.Api.Controllers;

[Route("api/me/journal")]
[ApiController]
public class MyJournalController(IMediator mediator) : ControllerBase
{
    public async Task<IActionResult> Get([FromQuery] DateTime date, CancellationToken cancellationToken)
    {
        return Ok(await mediator.Send(new GetUserJournalQuery(date), cancellationToken));
    }

    [HttpPut]
    public async Task<IActionResult> Post([FromBody] CreateJournalLogRequest createJournalLogRequest,
        CancellationToken cancellationToken)
    {
        return Ok(await mediator.Send(new CreateOrUpdateJournalLogCommand(createJournalLogRequest), cancellationToken));
    }
}