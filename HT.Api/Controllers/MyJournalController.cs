using HT.Application.Dto;
using HT.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HT.Api.Controllers;

[Route("api/me/journal")]
[ApiController]
public class MyJournalController(IUserJournalService userJournalService) : ControllerBase
{
    public async Task<IActionResult> Get([FromQuery] DateTime date, CancellationToken cancellationToken) =>
        Ok(await userJournalService.GetAsync(date, cancellationToken));

    [HttpPut]
    public async Task<IActionResult> Put([FromBody] CreateJournalLogRequest request,
        CancellationToken cancellationToken)
    {
        await userJournalService.CreateAsync(request, cancellationToken);
        return NoContent();
    }
}