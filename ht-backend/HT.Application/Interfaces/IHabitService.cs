using HT.Application.Dto;

namespace HT.Application.Interfaces;

public interface IHabitService
{ 
    Task<List<HabitDto>> GetAsync(CancellationToken cancellationToken = default);
    Task<HabitDetailsDto?> GetDetailsAsync(Guid habitId, CancellationToken cancellationToken = default);
}