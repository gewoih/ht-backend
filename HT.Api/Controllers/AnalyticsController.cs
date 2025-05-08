using HT.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HT.Api.Controllers;

[Route("api/me/[controller]")]
[ApiController]
[Authorize]
public class AnalyticsController(IUserAnalyticsService userAnalyticsService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] DateOnly fromDate, [FromQuery] DateOnly toDate, CancellationToken ct) =>
        Ok(await userAnalyticsService.GetDateScores(fromDate, toDate, ct));
}