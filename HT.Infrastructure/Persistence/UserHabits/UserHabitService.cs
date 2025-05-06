using HT.Application.Dto;
using HT.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HT.Infrastructure.Persistence.UserHabits;

public class UserHabitService(HtContext context, ICurrentUserService currentUserService) : IUserHabitService
{
    public async Task<List<Guid>> GetIdsAsync(CancellationToken cancellationToken = default)
    {
        var currentUserId = currentUserService.GetId();
        return await context.UserHabits
            .Where(habit => habit.UserId == currentUserId)
            .Select(userHabit => userHabit.HabitId)
            .ToListAsync(cancellationToken: cancellationToken);
    }

    public async Task<List<HabitDto>> GetAsync(CancellationToken cancellationToken = default)
    {
        var currentUserId = currentUserService.GetId();
        return await context.UserHabits
            .Where(habit => habit.UserId == currentUserId)
            .Select(userHabit => new HabitDto(userHabit.Habit!.Id, userHabit.Habit.Category, userHabit.Habit.Name))
            .ToListAsync(cancellationToken: cancellationToken);
    }
}