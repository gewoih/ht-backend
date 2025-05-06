using HT.Application.Insights.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HT.Api.Controllers;

[Route("api/me/insight")]
[ApiController]
public class MyInsightController(IInsightService insightService) : ControllerBase
{
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        return Ok(await insightService.GetInsightsAsync(cancellationToken));
    }
}