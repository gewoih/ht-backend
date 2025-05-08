using HT.Application.Dto;
using HT.Application.Interfaces;
using HT.Application.Mappers;
using Microsoft.EntityFrameworkCore;

namespace HT.Infrastructure.Persistence;

public sealed class HabitService(HtContext context) : IHabitService
{
    public async Task<List<HabitDto>> GetAsync(CancellationToken cancellationToken = default)
    {
        return await context.Habits
            .Select(habit => habit.ToDto())
            .ToListAsync(cancellationToken: cancellationToken);
    }

    public async Task<HabitDetailsDto?> GetDetailsAsync(Guid habitId, CancellationToken cancellationToken = default)
    {
        return await context.Habits
            .Where(habit => habit.Id == habitId)
            .Select(habit => habit.ToDetailsDto())
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
    }
}