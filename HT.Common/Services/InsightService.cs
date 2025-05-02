using HT.Common.Database;
using HT.Common.Dto;

namespace HT.Common.Services;

public class InsightService(
    JournalService journalService,
    HabitService habitService,
    NeuralEngine neuralEngine,
    HtContext context)
{
    public async Task GetInsightsAsync()
    {
        var journalLogs = await journalService.GetAllAsync();
        var filteredHabitLogs = GetFilteredHabitLogs(journalLogs);
        var allHabits = await habitService.GetAllAsync();
        var x = neuralEngine.GetHabitsImportance(filteredHabitLogs, allHabits);
    }

    private static IEnumerable<HabitLogDto> GetFilteredHabitLogs(List<JournalLogDto> journalLogs)
    {
        return journalLogs
            .SelectMany(journalLog => journalLog.HabitLogs)
            .GroupBy(habitLog => habitLog.HabitId)
            .Where(group =>
            {
                var zeroCount = group.Count(habitLog => habitLog.Value == false);
                var nonZeroCount = group.Count(habitLog => habitLog.Value);
                return zeroCount >= 5 && nonZeroCount >= 5;
            })
            .SelectMany(group => group);
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