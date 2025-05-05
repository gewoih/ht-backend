using HT.Domain.Entities;
using HT.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HT.Infrastructure.Persistence.Journals;

public class UserJournalRepository(HtContext context) : IUserJournalRepository
{
    public async Task<JournalLog?> GetByUserAndDateAsync(Guid userId, DateTime dateUtc, CancellationToken ct = default)
    {
        return await context.JournalLogs
            .Include(j => j.HabitLogs)
            .FirstOrDefaultAsync(j => j.UserId == userId && j.Date == dateUtc, ct);
    }

    public async Task CreateAsync(JournalLog journalLog, CancellationToken cancellationToken = default)
    {
        await context.JournalLogs.AddAsync(journalLog, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }
}