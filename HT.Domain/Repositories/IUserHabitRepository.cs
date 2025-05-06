namespace HT.Domain.Repositories;

public interface IUserHabitRepository
{
    Task ReplaceAsync(Guid userId, List<Guid> habitIds, CancellationToken cancellationToken = default);
}