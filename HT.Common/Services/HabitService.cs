using HT.Common.Database;
using HT.Common.Dto;
using Microsoft.EntityFrameworkCore;

namespace HT.Common.Services;

public sealed class HabitService(HtContext context)
{
    public async Task<List<HabitDto>> GetAllAsync()
    {
        return await context.Habits
            .OrderBy(habit => habit.Name)
            .Select(habit => new HabitDto(habit.Id, habit.Category, habit.Name))
            .ToListAsync();
    }
}