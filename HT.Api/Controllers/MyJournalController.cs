using HT.Common.Dto;
using HT.Common.Services;
using Microsoft.AspNetCore.Mvc;

namespace HT.Api.Controllers;

[Route("api/me/journal")]
[ApiController]
public class MyJournalController(UserJournalService userJournalService) : ControllerBase
{
    public async Task<IActionResult> Get([FromQuery] DateTime date)
    {
        var journalLogDto = await userJournalService.GetAsync(date);
        if (journalLogDto == null)
            return NotFound();
            
        return Ok(journalLogDto);
    }
        
    [HttpPut]
    public async Task<IActionResult> Post([FromBody] CreateJournalLogRequest createJournalLogRequest)
    {
        await userJournalService.CreateOrUpdateAsync(createJournalLogRequest);
        return Ok();
    }
}