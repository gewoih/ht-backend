using HT.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HT.Api.Controllers;

[Route("api/me/insight")]
[ApiController]
[Authorize]
public class MyInsightController(IInsightService insightService) : ControllerBase
{
    public async Task<IActionResult> Get(CancellationToken cancellationToken) =>
        Ok(await insightService.GetInsightsAsync(cancellationToken));
}