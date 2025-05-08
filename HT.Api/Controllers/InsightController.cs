using HT.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HT.Api.Controllers;

[Route("api/me/[Controller]")]
[ApiController]
[Authorize]
public class InsightController(IInsightService insightService) : ControllerBase
{
    public async Task<IActionResult> Get(CancellationToken cancellationToken) =>
        Ok(await insightService.GetInsightsAsync(cancellationToken));
}