using HT.Common.Database;
using HT.Common.Dto;
using Microsoft.EntityFrameworkCore;

namespace HT.Common.Services;

public sealed class HabitService(HtContext context)
{
    public async Task<List<HabitDto>> GetAsync()
    {
        return await context.Habits
            .Select(habit => new HabitDto(habit.Id, habit.Category, habit.Name))
            .ToListAsync();
    }

    public async Task<HabitDetailsDto?> GetDetailsAsync(Guid habitId)
    {
        return await context.Habits
            .Where(habit => habit.Id == habitId)
            .Select(habit => new HabitDetailsDto(habit.Description, habit.Recommendation))
            .FirstOrDefaultAsync();
    }
}