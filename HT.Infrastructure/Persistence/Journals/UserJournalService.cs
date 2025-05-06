using HT.Application.Dto;
using HT.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HT.Infrastructure.Persistence.Journals;

public sealed class UserJournalService(HtContext context) : IUserJournalService
{
    //TODO: Mappers
    public async Task<JournalLogDto?> GetAsync(DateTime date, CancellationToken cancellationToken = default)
    {
        return await context.JournalLogs
            .Where(journalLog => journalLog.Date == date.Date.ToUniversalTime())
            .Select(journalLog =>
                new JournalLogDto(journalLog.Date, journalLog.HealthScore, journalLog.EnergyScore, journalLog.MoodScore,
                    journalLog.HabitLogs.Select(habitLog => new HabitLogDto(habitLog.HabitId, habitLog.Value != 0f))))
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
    }

    public async Task<List<JournalLogDto>> GetLogsAsync(CancellationToken cancellationToken = default)
    {
        return await context.JournalLogs
            .Select(journalLog =>
                new JournalLogDto(journalLog.Date, journalLog.HealthScore, journalLog.EnergyScore, journalLog.MoodScore,
                    journalLog.HabitLogs.Select(habitLog => new HabitLogDto(habitLog.HabitId, habitLog.Value != 0f))))
            .ToListAsync(cancellationToken: cancellationToken);
    }
}