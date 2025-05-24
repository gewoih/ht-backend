using HT.Core.Dto;
using HT.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HT.Core.Infrastructure.Persistence;

public class LeaderboardService(ICurrentUserService currentUserService, HtContext context) : ILeaderboardService
{
    public async Task<LeaderboardDto> GetAsync(DateOnly fromDate, DateOnly toDate, CancellationToken ct = default)
    {
        var userScores = await context.JournalLogs
            .Where(journalLog => journalLog.Date >= fromDate && journalLog.Date <= toDate)
            .GroupBy(journalLog => journalLog.UserId)
            .Select(group => new
            {
                UserId = group.Key,
                Score = group.Sum(journalLog =>
                    journalLog.HabitLogs.Where(habitLog => habitLog.Value).Sum(habitLog => habitLog.Habit.Impact))
            })
            .OrderByDescending(group => group.Score)
            .ToDictionaryAsync(key => key.UserId, value => value.Score, cancellationToken: ct);

        var userIds = userScores.Keys.ToList();
        var currentUserId = currentUserService.GetUserId();
        var currentUserProfile = await currentUserService.GetUserProfileAsync(ct);

        var usernames = await context.Users
            .Where(u => userIds.Contains(u.Id))
            .ToDictionaryAsync(u => u.Id, u => u.UserName, cancellationToken: ct);

        var rankedUsers = new List<LeaderboardUserDto>();
        foreach (var ((userId, userScore), index) in userScores.Select((pair, i) => (pair, i)))
        {
            if (!usernames.TryGetValue(userId, out var username))
                continue;

            rankedUsers.Add(new LeaderboardUserDto(userId, username!, userScore, index + 1));
        }

        var topUsers = rankedUsers.Take(10).ToList();
        var currentUser = rankedUsers.FirstOrDefault(x => x.UserId == currentUserId) ??
                          new LeaderboardUserDto(currentUserId, currentUserProfile!.Username, 0, 0);

        return new LeaderboardDto(topUsers, currentUser);
    }
}