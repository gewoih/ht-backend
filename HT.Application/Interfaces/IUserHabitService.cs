using HT.Application.Habits.Dto;

namespace HT.Application.Habits.Interfaces;

public interface IUserHabitService
{
    Task<List<Guid>> GetIdsAsync(CancellationToken cancellationToken = default);
    Task<List<HabitDto>> GetAsync(CancellationToken cancellationToken = default);
}