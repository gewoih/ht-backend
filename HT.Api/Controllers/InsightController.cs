using HT.Common.Services;
using Microsoft.AspNetCore.Mvc;

namespace HT.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class InsightController(InsightService insightService) : ControllerBase
{
    public async Task<IActionResult> Get()
    {
        var insights = await insightService.GetInsightsAsync();
        return Ok(insights);
    }
}