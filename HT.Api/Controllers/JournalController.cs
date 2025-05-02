using HT.Common.Dto;
using HT.Common.Services;
using Microsoft.AspNetCore.Mvc;

namespace HT.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class JournalController(JournalService journalService) : ControllerBase
{
    public async Task<IActionResult> Get([FromQuery] DateTime date)
    {
        var journalLogDto = await journalService.GetAsync(date);
        if (journalLogDto == null)
            return NotFound();
            
        return Ok(journalLogDto);
    }
        
    [HttpPut]
    public async Task<IActionResult> Post([FromBody] CreateJournalLogRequest createJournalLogRequest)
    {
        await journalService.CreateOrUpdateAsync(createJournalLogRequest);
        return Ok();
    }
}