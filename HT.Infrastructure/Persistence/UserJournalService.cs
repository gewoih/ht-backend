using HT.Application.Dto;
using HT.Application.Dto.Requests;
using HT.Application.Interfaces;
using HT.Application.Mappers;
using HT.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HT.Infrastructure.Persistence;

public sealed class UserJournalService(HtContext context, ICurrentUserService currentUserService) : IUserJournalService
{
    public async Task<JournalLogDto?> GetAsync(DateTime date, CancellationToken cancellationToken = default)
    {
        return await context.JournalLogs
            .Where(journalLog => journalLog.Date == date.Date.ToUniversalTime())
            .Select(journalLog => journalLog.ToDto())
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
            existingLog.HabitLogs = request.HabitLogs.Select(habitLog => habitLog.ToDomain()).ToList();
        }
        else
        {
            var habitLogs = request.HabitLogs.Select(habitLog => habitLog.ToDomain()).ToList();
            var newLog = new JournalLog(request.UserId, journalLogDateUtc, request.HealthScore, request.EnergyScore,
                request.MoodScore, habitLogs);
            
            await context.JournalLogs.AddAsync(newLog, cancellationToken);
        }

        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<JournalLogDto>> GetLogsAsync(CancellationToken cancellationToken = default)
    {
        return await context.JournalLogs
            .Select(journalLog => journalLog.ToDto())
            .ToListAsync(cancellationToken: cancellationToken);
    }

    private async Task<JournalLog?> GetByUserAndDateAsync(Guid userId, DateTime dateUtc, CancellationToken ct = default)
    {
        return await context.JournalLogs
            .Include(j => j.HabitLogs)
            .FirstOrDefaultAsync(j => j.UserId == userId && j.Date == dateUtc, ct);
    }
}