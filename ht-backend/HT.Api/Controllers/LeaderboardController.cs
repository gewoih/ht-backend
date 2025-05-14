using HT.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HT.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class LeaderboardController(ILeaderboardService leaderboardService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get(DateOnly fromDate, DateOnly toDate) =>
        Ok(await leaderboardService.GetAsync(fromDate, toDate));
}