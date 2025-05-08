using HT.Application.Dto;
using HT.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HT.Infrastructure.Persistence;

public class UserAnalyticsService(ICurrentUserService currentUserService, HtContext context) : IUserAnalyticsService
{
    public async Task<List<DateScoreDto>> GetDateScores(DateOnly fromDate, DateOnly toDate,
        CancellationToken ct = default)
    {
        var currentUserId = currentUserService.GetUserId();
        return await context.JournalLogs
            .Where(journalLog => journalLog.UserId == currentUserId &&
                                 journalLog.Date >= fromDate &&
                                 journalLog.Date <= toDate)
            .Select(journalLog => new DateScoreDto(journalLog.Date, journalLog.Score))
            .ToListAsync(cancellationToken: ct);
    }
}