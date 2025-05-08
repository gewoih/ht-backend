using HT.Application.Dto.Requests;
using HT.Application.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HT.Api.Controllers;

[Route("api/me/journal")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class MyJournalController(IUserJournalService userJournalService) : ControllerBase
{
    public async Task<IActionResult> Get([FromQuery] DateTime date, CancellationToken cancellationToken)
    {
        var journalLog = await userJournalService.GetAsync(date, cancellationToken);
        return journalLog != null ? Ok(journalLog) : NotFound();
    }

    [HttpPut]
    public async Task<IActionResult> Put([FromBody] CreateJournalLogRequest request,
        CancellationToken cancellationToken)
    {
        await userJournalService.CreateAsync(request, cancellationToken);
        return NoContent();
    }
}