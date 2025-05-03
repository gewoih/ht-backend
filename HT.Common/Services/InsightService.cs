using HT.Common.Dto;

namespace HT.Common.Services;

public class InsightService(
    HabitService habitService,
    JournalService journalService,
    NeuralEngine neuralEngine)
{
    public async Task<List<InsightDto>> GetInsightsAsync()
    {
        var habits = await habitService.GetAllAsync();

        var journalLogs = await journalService.GetLogsAsync();
        var filteredHabitLogs = GetFilteredJournalLogs(journalLogs);
        var importanceMap = neuralEngine.GetHabitsImportance(filteredHabitLogs);

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
            .Select(h => h.HabitId)
            .Distinct()
            .ToHashSet();

        var filteredJournalLogs = journalLogs.Select(journalLog =>
                new JournalLogDto(journalLog.Date, journalLog.HealthScore, journalLog.EnergyScore, journalLog.MoodScore,
                    journalLog.HabitLogs.Where(h => allowedHabitIds.Contains(h.HabitId))))
            .Where(j => j.HabitLogs.Any())
            .ToList();

        return filteredJournalLogs;
    }


    // private static Dictionary<string, double> GetBaselineHabitsAnalysis(List<HabitDto> habits, List<JournalLog> journalLogs)
    // {
    //     var byHabit = new Dictionary<string, double>();
    //     foreach (var habit in habits)
    //     {
    //         var positiveScores = journalLogs
    //             .Where(j => j.HabitLogs?.Any(h => h.HabitId == habit.Id && h.Value > 0) == true)
    //             .Select(j => j.Score)
    //             .ToList();
    //
    //         var negativeScores = journalLogs
    //             .Where(j => j.HabitLogs?.Any(h => h.HabitId == habit.Id && h.Value == 0) == true)
    //             .Select(j => j.Score)
    //             .ToList();
    //
    //         var positiveAvg = positiveScores.Count > 0 ? positiveScores.Average() : 0;
    //         var negativeAvg = negativeScores.Count > 0 ? negativeScores.Average() : 0;
    //         var difference = positiveAvg - negativeAvg;
    //         
    //         byHabit[habit.Name] = difference;
    //     }
    //
    //     return byHabit;
    // }
}