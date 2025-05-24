using HT.Core.Dto;

namespace HT.Core.Interfaces;

public interface IUserHabitService
{
    Task<List<Guid>> GetIdsAsync(CancellationToken cancellationToken = default);
    Task<List<HabitDto>> GetAsync(CancellationToken cancellationToken = default);
    Task ReplaceAsync(List<Guid> habitIds, CancellationToken cancellationToken = default);
}