using HT.Application.Dto;
using HT.Application.Dto.Requests;
using HT.Application.Interfaces;
using HT.Application.Mappers;
using HT.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HT.Infrastructure.Persistence;

public sealed class UserJournalService(HtContext context, ICurrentUserService currentUserService) : IUserJournalService
{
    private readonly Guid _currentUserId = currentUserService.GetUserId();
    
    public async Task<JournalLogDto?> GetAsync(DateOnly date, CancellationToken cancellationToken = default)
    {
        return await context.JournalLogs
            .Include(journalLog => journalLog.HabitLogs)
            .Where(journalLog => journalLog.Date == date && journalLog.UserId == _currentUserId)
            .Select(journalLog => journalLog.ToDto())
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
    }

    public async Task CreateAsync(CreateJournalLogRequest request, CancellationToken cancellationToken = default)
    {
        var existingLog = await GetByUserAndDateAsync(_currentUserId, request.Date, cancellationToken);
        if (existingLog != null)
        {
            existingLog.Score = request.DailyScore;
            existingLog.HabitLogs = request.HabitLogs.Select(habitLog => habitLog.ToDomain()).ToList();
        }
        else
        {
            var dailyScore = request.DailyScore;
            var habitLogs = request.HabitLogs.Select(habitLog => habitLog.ToDomain()).ToList();
            var newLog = new JournalLog(_currentUserId, request.Date, dailyScore, habitLogs);
            
            await context.JournalLogs.AddAsync(newLog, cancellationToken);
        }

        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<JournalLogDto>> GetAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await context.JournalLogs
            .Include(journalLog => journalLog.HabitLogs)
            .Where(journalLog => journalLog.UserId == userId)
            .Select(journalLog => journalLog.ToDto())
            .ToListAsync(cancellationToken: cancellationToken);
    }

    private async Task<JournalLog?> GetByUserAndDateAsync(Guid userId, DateOnly date, CancellationToken ct = default)
    {
        return await context.JournalLogs
            .Include(journalLog => journalLog.HabitLogs)
            .SingleOrDefaultAsync(journalLog => journalLog.UserId == userId && journalLog.Date == date, ct);
    }
}