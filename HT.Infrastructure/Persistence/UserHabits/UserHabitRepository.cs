using HT.Domain.Entities;
using HT.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HT.Infrastructure.Persistence.UserHabits;

public class UserHabitRepository(HtContext context) : IUserHabitRepository
{
    public async Task ReplaceAsync(Guid userId, List<Guid> habitIds, CancellationToken cancellationToken = default)
    {
        var userExists = await context.Users.AnyAsync(u => u.Id == userId, cancellationToken: cancellationToken);
        if (!userExists)
            throw new Exception("User not found");

        var existingUserHabits = await context.UserHabits
            .Where(uh => uh.UserId == userId)
            .ToListAsync(cancellationToken: cancellationToken);

        context.UserHabits.RemoveRange(existingUserHabits);

        var newUserHabits = habitIds.Select(habitId => new UserHabit
        {
            UserId = userId,
            HabitId = habitId,
        });

        await context.UserHabits.AddRangeAsync(newUserHabits, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }
}