using HT.Common.Services;
using Microsoft.AspNetCore.Mvc;

namespace HT.Api.Controllers;

[Route("api/me/insight")]
[ApiController]
public class MyInsightController(InsightService insightService) : ControllerBase
{
    public async Task<IActionResult> Get()
    {
        var insights = await insightService.GetInsightsAsync();
        return Ok(insights);
    }
}