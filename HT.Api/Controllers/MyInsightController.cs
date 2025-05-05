using HT.Application.Insights.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HT.Api.Controllers;

[Route("api/me/insight")]
[ApiController]
public class MyInsightController(IMediator mediator) : ControllerBase
{
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        return Ok(await mediator.Send(new GetInsightsQuery(), cancellationToken));
    }
}