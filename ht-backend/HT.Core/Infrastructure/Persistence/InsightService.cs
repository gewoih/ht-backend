using HT.Core.Dto;
using HT.Core.Interfaces;

namespace HT.Core.Infrastructure.Persistence;

public class InsightService(
    IHabitService habitService,
    IUserJournalService userJournalService,
    ICurrentUserService currentUserService,
    NeuralEngine neuralEngine) : IInsightService
{
    public async Task<List<InsightDto>> GetInsightsAsync(CancellationToken cancellationToken = default)
    {
        var currentUser = currentUserService.GetUserId();
        var habits = await habitService.GetAsync(cancellationToken);
        var journalLogs = await userJournalService.GetAsync(currentUser, cancellationToken);
        var filteredHabitLogs = GetFilteredJournalLogs(journalLogs);
        var importanceMap = neuralEngine.GetHabitsImportance(filteredHabitLogs);

        var insights = importanceMap
            .Select(kvp => new InsightDto(habits.First(habit => habit.Id == kvp.Key), kvp.Value))
            .ToList();

        return insights;
    }

    private static List<JournalLogDto> GetFilteredJournalLogs(List<JournalLogDto> journalLogs)
    {
        var filteredHabitLogs = journalLogs
            .SelectMany(j => j.HabitLogs)
            .ToList();

        var allowedHabitIds = filteredHabitLogs
            .Select(habitLog => habitLog.HabitId)
            .Distinct()
            .ToHashSet();

        var filteredJournalLogs = new List<JournalLogDto>();
        foreach (var journalLog in journalLogs)
        {
            var habitLogs = journalLog.HabitLogs
                .Where(habitLog => allowedHabitIds.Contains(habitLog.HabitId));
            
            journalLog.UpdateHabitLogs(habitLogs);
            if (journalLog.HabitLogs.Any())
                filteredJournalLogs.Add(journalLog);
        }

        return filteredJournalLogs;
    }
}