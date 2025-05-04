using HT.Common.Database;
using HT.Common.Dto;
using HT.Common.Entities;
using Microsoft.EntityFrameworkCore;

namespace HT.Common.Services;

public class UserHabitService(HtContext context, CurrentUserService currentUserService)
{
    public async Task<List<Guid>> GetIdsAsync()
    {
        var currentUserId = currentUserService.GetCurrentUserId();
        return await context.UserHabits
            .Where(habit => habit.UserId == currentUserId)
            .Select(userHabit => userHabit.HabitId)
            .ToListAsync();
    }
    
    public async Task<List<HabitDto>> GetAsync()
    {
        var currentUserId = currentUserService.GetCurrentUserId();
        return await context.UserHabits
            .Where(habit => habit.UserId == currentUserId)
            .Select(userHabit => new HabitDto(userHabit.Habit!.Id, userHabit.Habit.Category, userHabit.Habit.Name))
            .ToListAsync();
    }

    public async Task ReplaceAsync(List<Guid> habitIds)
    {
        var currentUserId = currentUserService.GetCurrentUserId();
        var userExists = await context.Users.AnyAsync(u => u.Id == currentUserId);
        if (!userExists)
            throw new Exception("User not found");

        var existingUserHabits = await context.UserHabits
            .Where(uh => uh.UserId == currentUserId)
            .ToListAsync();

        context.UserHabits.RemoveRange(existingUserHabits);

        var newUserHabits = habitIds.Select(habitId => new UserHabit
        {
            UserId = currentUserId,
            HabitId = habitId,
        });

        await context.UserHabits.AddRangeAsync(newUserHabits);
        await context.SaveChangesAsync();
    }
}