using HT.Application.Habits.Dto;
using HT.Application.Habits.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HT.Infrastructure.Persistence.Habits;

public sealed class HabitService(HtContext context) : IHabitService
{
    public async Task<List<HabitDto>> GetAsync(CancellationToken cancellationToken = default)
    {
        return await context.Habits
            .Select(habit => new HabitDto(habit.Id, habit.Category, habit.Name))
            .ToListAsync(cancellationToken: cancellationToken);
    }

    public async Task<HabitDetailsDto?> GetDetailsAsync(Guid habitId, CancellationToken cancellationToken = default)
    {
        return await context.Habits
            .Where(habit => habit.Id == habitId)
            .Select(habit => new HabitDetailsDto(habit.Description, habit.Recommendation))
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
    }
}