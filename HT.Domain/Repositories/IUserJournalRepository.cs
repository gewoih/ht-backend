using HT.Domain.Entities;

namespace HT.Domain.Repositories;

public interface IUserJournalRepository
{
    Task<JournalLog?> GetByUserAndDateAsync(Guid userId, DateTime dateUtc, CancellationToken ct = default);
    Task CreateAsync(JournalLog journalLog, CancellationToken cancellationToken = default);
}