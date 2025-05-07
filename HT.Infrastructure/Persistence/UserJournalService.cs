using HT.Application.Dto;
using HT.Application.Dto.Requests;
using HT.Application.Interfaces;
using HT.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HT.Infrastructure.Persistence;

public sealed class UserJournalService(HtContext context, ICurrentUserService currentUserService) : IUserJournalService
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

    public async Task CreateAsync(CreateJournalLogRequest request, CancellationToken cancellationToken = default)
    {
        var journalLogDateUtc = request.Date.ToUniversalTime();
        var currentUserId = currentUserService.GetId();

        var existingLog = await GetByUserAndDateAsync(currentUserId, journalLogDateUtc, cancellationToken);

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

            await context.JournalLogs.AddAsync(newLog, cancellationToken);
        }
        
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<JournalLogDto>> GetLogsAsync(CancellationToken cancellationToken = default)
    {
        return await context.JournalLogs
            .Select(journalLog =>
                new JournalLogDto(journalLog.Date, journalLog.HealthScore, journalLog.EnergyScore, journalLog.MoodScore,
                    journalLog.HabitLogs.Select(habitLog => new HabitLogDto(habitLog.HabitId, habitLog.Value != 0f))))
            .ToListAsync(cancellationToken: cancellationToken);
    }
    
    private async Task<JournalLog?> GetByUserAndDateAsync(Guid userId, DateTime dateUtc, CancellationToken ct = default)
    {
        return await context.JournalLogs
            .Include(j => j.HabitLogs)
            .FirstOrDefaultAsync(j => j.UserId == userId && j.Date == dateUtc, ct);
    }
}