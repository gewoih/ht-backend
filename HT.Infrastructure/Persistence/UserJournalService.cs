using HT.Application.Dto;
using HT.Application.Dto.Requests;
using HT.Application.Interfaces;
using HT.Application.Mappers;
using HT.Domain.Entities;
using HT.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace HT.Infrastructure.Persistence;

public sealed class UserJournalService(HtContext context, ICurrentUserService currentUserService) : IUserJournalService
{
    public async Task<JournalLogDto?> GetAsync(DateOnly date, CancellationToken cancellationToken = default)
    {
        return await context.JournalLogs
            .Include(journalLog => journalLog.HabitLogs)
            .Where(journalLog => journalLog.Date == date)
            .Select(journalLog => journalLog.ToDto())
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
    }

    public async Task CreateAsync(CreateJournalLogRequest request, CancellationToken cancellationToken = default)
    {
        var currentUserId = currentUserService.GetUserId();
        var existingLog = await GetByUserAndDateAsync(currentUserId, request.Date, cancellationToken);

        if (existingLog != null)
        {
            existingLog.Score = request.DailyScore;
            existingLog.HabitLogs = request.HabitLogs.Select(habitLog => habitLog.ToDomain()).ToList();
        }
        else
        {
            var dailyScore = request.DailyScore;
            var habitLogs = request.HabitLogs.Select(habitLog => habitLog.ToDomain()).ToList();
            var newLog = new JournalLog(currentUserId, request.Date, dailyScore, habitLogs);
            
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

    private async Task<JournalLog?> GetByUserAndDateAsync(Guid userId, DateOnly date, CancellationToken ct = default)
    {
        return await context.JournalLogs
            .Include(j => j.HabitLogs)
            .FirstOrDefaultAsync(j => j.UserId == userId && j.Date == date, ct);
    }
}