using HT.Application.Common.Interfaces;
using HT.Application.Journal.Dto;
using HT.Application.Journal.Interfaces;
using HT.Domain.Entities;
using HT.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace HT.Api.Controllers;

[Route("api/me/journal")]
[ApiController]
public class MyJournalController(
    IUserJournalService userJournalService,
    ICurrentUserService currentUserService,
    IUserJournalRepository userJournalRepository) : ControllerBase
{
    public async Task<IActionResult> Get([FromQuery] DateTime date, CancellationToken cancellationToken)
    {
        return Ok(await userJournalService.GetAsync(date, cancellationToken));
    }

    [HttpPut]
    public async Task<IActionResult> Post([FromBody] CreateJournalLogRequest request,
        CancellationToken cancellationToken)
    {
        var journalLogDateUtc = request.Date.ToUniversalTime();
        var currentUserId = currentUserService.GetId();

        var existingLog =
            await userJournalRepository.GetByUserAndDateAsync(currentUserId, journalLogDateUtc, cancellationToken);

        if (existingLog != null)
        {
            existingLog.UpdateScores(request.HealthScore, request.EnergyScore, request.MoodScore);

            existingLog.HabitLogs = request.HabitLogs.Select(habitLog => new HabitLog
            {
                HabitId = habitLog.HabitId,
                Value = habitLog.Value ? 1.0f : 0.0f
            }).ToList();
        }
        else
        {
            var newLog = new JournalLog
            {
                UserId = request.UserId,
                Date = journalLogDateUtc,
                HealthScore = request.HealthScore,
                EnergyScore = request.EnergyScore,
                MoodScore = request.MoodScore,
                HabitLogs = request.HabitLogs.Select(habitLog => new HabitLog
                {
                    HabitId = habitLog.HabitId,
                    Value = habitLog.Value ? 1.0f : 0.0f
                }).ToList()
            };

            await userJournalRepository.CreateAsync(newLog, cancellationToken);
        }

        return Ok();
    }
}