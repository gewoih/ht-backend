using HT.Common.Database;
using HT.Common.Dto;
using HT.Common.Entities;
using Microsoft.EntityFrameworkCore;

namespace HT.Common.Services;

public sealed class JournalService(HtContext context)
{
    //TODO: Mappers
    public async Task<JournalLogDto?> GetAsync(DateTime date)
    {
        return await context.JournalLogs
            .Where(journalLog => journalLog.Date == date.Date.ToUniversalTime())
            .Select(journalLog =>
                new JournalLogDto(journalLog.Date, journalLog.HealthScore, journalLog.EnergyScore, journalLog.MoodScore,
                    journalLog.HabitLogs.Select(habitLog => new HabitLogDto(habitLog.HabitId, habitLog.Value != 0f))))
            .FirstOrDefaultAsync();
    }

    public async Task<List<JournalLogDto>> GetLogsAsync()
    {
        return await context.JournalLogs
            .Select(journalLog =>
                new JournalLogDto(journalLog.Date, journalLog.HealthScore, journalLog.EnergyScore, journalLog.MoodScore,
                    journalLog.HabitLogs.Select(habitLog => new HabitLogDto(habitLog.HabitId, habitLog.Value != 0f))))
            .ToListAsync();
    }

    public async Task CreateOrUpdateAsync(CreateJournalLogRequest request)
    {
        var journalLogDateUtc = request.Date.ToUniversalTime();

        var existingLog = await context.JournalLogs
            .Include(j => j.HabitLogs)
            .FirstOrDefaultAsync(j => j.UserId == request.UserId && j.Date == journalLogDateUtc);

        if (existingLog != null)
        {
            existingLog.HealthScore = request.HealthScore;
            existingLog.EnergyScore = request.EnergyScore;
            existingLog.MoodScore = request.MoodScore;

            context.HabitLogs.RemoveRange(existingLog.HabitLogs);
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

            await context.JournalLogs.AddAsync(newLog);
        }

        await context.SaveChangesAsync();
    }
}