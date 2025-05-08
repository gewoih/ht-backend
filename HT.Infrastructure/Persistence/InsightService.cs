using HT.Application.Dto;
using HT.Application.Interfaces;

namespace HT.Infrastructure.Persistence;

public class InsightService(
    IHabitService habitService,
    IUserJournalService userJournalService,
    NeuralEngine neuralEngine) : IInsightService
{
    public async Task<List<InsightDto>> GetInsightsAsync(CancellationToken cancellationToken = default)
    {
        var habits = await habitService.GetAsync(cancellationToken);

        var journalLogs = await userJournalService.GetLogsAsync(cancellationToken);
        var filteredHabitLogs = GetFilteredJournalLogs(journalLogs);
        var importanceMap = neuralEngine.GetHabitsImportance(filteredHabitLogs);

        //TODO: Маппер
        var insights = importanceMap
            .Select(kvp => new InsightDto(habits.First(habit => habit.Id == kvp.Key), kvp.Value))
            .OrderByDescending(insight => insight.Influence)
            .ToList();

        return insights;
    }

    private static List<JournalLogDto> GetFilteredJournalLogs(List<JournalLogDto> journalLogs)
    {
        var filteredHabitLogs = journalLogs
            .SelectMany(j => j.HabitLogs)
            .GroupBy(h => h.HabitId)
            .Where(group =>
            {
                var zeroCount = group.Count(h => h.Value == false);
                var nonZeroCount = group.Count(h => h.Value);
                return zeroCount >= 5 && nonZeroCount >= 5;
            })
            .SelectMany(g => g)
            .ToList();

        var allowedHabitIds = filteredHabitLogs
            .Select(habitLog => habitLog.HabitId)
            .Distinct()
            .ToHashSet();

        //TODO: Маппер
        var filteredJournalLogs = journalLogs.Select(journalLog =>
                new JournalLogDto(journalLog.Date, journalLog.HealthScore, journalLog.EnergyScore, journalLog.MoodScore,
                    journalLog.HabitLogs.Where(habitLog => allowedHabitIds.Contains(habitLog.HabitId))))
            .Where(j => j.HabitLogs.Any())
            .ToList();

        return filteredJournalLogs;
    }
}