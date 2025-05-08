using HT.Application.Dto;
using HT.Application.Interfaces;
using HT.Application.Mappers;
using HT.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HT.Infrastructure.Persistence;

public class UserHabitService(HtContext context, ICurrentUserService currentUserService) : IUserHabitService
{
    public async Task<List<Guid>> GetIdsAsync(CancellationToken cancellationToken = default)
    {
        var currentUserId = currentUserService.GetUserId();
        return await context.UserHabits
            .Where(habit => habit.UserId == currentUserId)
            .Select(userHabit => userHabit.HabitId)
            .ToListAsync(cancellationToken: cancellationToken);
    }

    public async Task<List<HabitDto>> GetAsync(CancellationToken cancellationToken = default)
    {
        var currentUserId = currentUserService.GetUserId();
        return await context.UserHabits
            .Where(habit => habit.UserId == currentUserId)
            .Select(userHabit => userHabit.ToDto())
            .ToListAsync(cancellationToken: cancellationToken);
    }

    public async Task ReplaceAsync(List<Guid> habitIds, CancellationToken cancellationToken = default)
    {
        var currentUserId = currentUserService.GetUserId();
        var existingUserHabits = await context.UserHabits
            .Where(uh => uh.UserId == currentUserId)
            .ToListAsync(cancellationToken: cancellationToken);

        context.UserHabits.RemoveRange(existingUserHabits);

        var newUserHabits = habitIds.Select(habitId => new UserHabit
        {
            UserId = currentUserId,
            HabitId = habitId,
        });

        await context.UserHabits.AddRangeAsync(newUserHabits, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }
}