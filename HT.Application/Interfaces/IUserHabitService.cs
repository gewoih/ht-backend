using HT.Application.Dto;

namespace HT.Application.Interfaces;

public interface IUserHabitService
{
    Task<List<Guid>> GetIdsAsync(CancellationToken cancellationToken = default);
    Task<List<HabitDto>> GetAsync(CancellationToken cancellationToken = default);
}