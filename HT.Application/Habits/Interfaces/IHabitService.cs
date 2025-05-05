using HT.Application.Habits.Dto;

namespace HT.Application.Habits.Interfaces;

public interface IHabitService
{ 
    Task<List<HabitDto>> GetAsync(CancellationToken cancellationToken = default);
    Task<HabitDetailsDto?> GetDetailsAsync(Guid habitId, CancellationToken cancellationToken = default);
}