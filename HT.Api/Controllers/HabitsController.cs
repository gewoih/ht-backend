using HT.Common.Services;
using Microsoft.AspNetCore.Mvc;

namespace HT.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HabitsController(HabitService habitService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await habitService.GetAllAsync());
        }

        [HttpGet("{id:guid}/details")]
        public async Task<IActionResult> GetDetails([FromRoute] Guid id)
        {
            var habitDetails = await habitService.GetDetailsAsync(id);
            if (habitDetails == null)
                return NotFound();
            
            return Ok(habitDetails);
        }
    }
}
