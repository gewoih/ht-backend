using HT.Application.Dto;
using HT.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HT.Infrastructure.Persistence;

public class LeaderboardService(ICurrentUserService currentUserService, HtContext context) : ILeaderboardService
{
    public async Task<LeaderboardDto> GetAsync(DateOnly fromDate, DateOnly toDate)
    {
        var userScores = await context.HabitLogs
            .Where(habitLog => habitLog.JournalLog.Date >= fromDate && habitLog.JournalLog.Date <= toDate)
            .GroupBy(habitLog => habitLog.JournalLog.UserId)
            .Select(group => new
            {
                UserId = group.Key,
                Score = group.Sum(habitLog => habitLog.Habit.Impact)
            })
            .OrderByDescending(x => x.Score)
            .ToListAsync();

        var allNeededUserIds = userScores.Select(x => x.UserId).ToList();
        var currentUserId = currentUserService.GetUserId();
        var currentUserProfile = await currentUserService.GetUserProfileAsync();

        var users = await context.Users
            .Where(u => allNeededUserIds.Contains(u.Id))
            .ToDictionaryAsync(u => u.Id, u => u.UserName);

        var rankedUsers = userScores
            .Select((x, index) => new LeaderboardUserDto(x.UserId, users[x.UserId], x.Score, index + 1))
            .ToList();

        var topUsers = rankedUsers.Take(10).ToList();
        var currentUser = rankedUsers.FirstOrDefault(x => x.UserId == currentUserId) ??
                          new LeaderboardUserDto(currentUserId, currentUserProfile.Email, 0, 0);

        return new LeaderboardDto(topUsers, currentUser);
    }
}