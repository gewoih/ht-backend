using HT.Application.Dto;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;

namespace HT.Infrastructure.Persistence;

public class NeuralEngine
{
    private const int Window = 7;
    private const float L2 = 0.1f;

    public Dictionary<Guid, double> GetHabitsImportance(List<JournalLogDto> journalLogs)
    {
        var habitIds = GetHabitIds(journalLogs);
        var avgScore = (float)journalLogs.Average(journalLogDto => journalLogDto.DailyScore.Total);

        var rows = BuildRows(journalLogs, habitIds, avgScore);
        var mlContext = new MLContext(seed: 228);

        var schemaDefinition = SchemaDefinition.Create(typeof(HabitData));
        schemaDefinition["Features"].ColumnType = new VectorDataViewType(NumberDataViewType.Single, habitIds.Count * 3);
        var dataView = mlContext.Data.LoadFromEnumerable(rows, schemaDefinition);
        var model = TrainModel(mlContext, dataView);

        return ExtractWeights(model, habitIds);
    }

    private static Dictionary<Guid, double> ExtractWeights(
        RegressionPredictionTransformer<LinearRegressionModelParameters> model,
        List<Guid> habitIds)
    {
        var coeffs = model.Model.Weights.ToArray();
        var dict = new Dictionary<Guid, double>(habitIds.Count);
        for (var i = 0; i < habitIds.Count; i++)
            dict[habitIds[i]] = coeffs[i];

        return dict;
    }

    private static RegressionPredictionTransformer<LinearRegressionModelParameters> TrainModel(MLContext ml,
        IDataView dataView)
    {
        var opt = new SdcaRegressionTrainer.Options
        {
            LabelColumnName = nameof(HabitData.Label),
            FeatureColumnName = nameof(HabitData.Features),
            MaximumNumberOfIterations = 1000,
            Shuffle = true,
            NumberOfThreads = 1,
            L2Regularization = L2
        };

        return ml.Regression.Trainers.Sdca(opt).Fit(dataView);
    }

    private static List<Guid> GetHabitIds(IEnumerable<JournalLogDto> logs) =>
        logs.SelectMany(j => j.HabitLogs)
            .Select(h => h.HabitId)
            .Distinct()
            .OrderBy(id => id.ToString())
            .ToList();

    private List<HabitData> BuildRows(
        List<JournalLogDto> journalLogDtos,
        List<Guid> habitIds,
        float avgScore)
    {
        var ordered = journalLogDtos.OrderBy(journalLogDto => journalLogDto.Date).ToList();

        // вспомогательные буферы для Sum7 и Streak
        var rolling = habitIds.ToDictionary(id => id, _ => new Queue<int>(7));
        var streaks = habitIds.ToDictionary(id => id, _ => 0);

        var rows = new List<HabitData>();

        for (var d = 0; d < ordered.Count; d++)
        {
            var features = ComposeFeatureVector(
                ordered[d], habitIds, rolling, streaks);

            if (d < Window - 1)
                continue;

            rows.Add(new HabitData
            {
                Features = features,
                Label = ordered[d].DailyScore.Total - avgScore
            });
        }

        return rows;
    }

    private static float[] ComposeFeatureVector(
        JournalLogDto log,
        IList<Guid> habitIds,
        IDictionary<Guid, Queue<int>> rolling,
        IDictionary<Guid, int> streaks)
    {
        var habitsCount = habitIds.Count;
        var today = new float[habitsCount];
        var sum7 = new float[habitsCount];
        var streak = new float[habitsCount];

        foreach (var habitLog in log.HabitLogs)
        {
            var idx = habitIds.IndexOf(habitLog.HabitId);
            var on = habitLog.Value ? 1 : -1;
            today[idx] = on;

            // Sum7
            var q = rolling[habitLog.HabitId];
            q.Enqueue(habitLog.Value ? 1 : 0);
            if (q.Count > Window) q.Dequeue();

            // Streak
            streaks[habitLog.HabitId] = habitLog.Value ? streaks[habitLog.HabitId] + 1 : 0;
        }

        for (var i = 0; i < habitsCount; i++)
        {
            var id = habitIds[i];
            sum7[i] = rolling[id].Sum();
            streak[i] = streaks[id];
        }

        var full = new float[habitsCount * 3];
        Buffer.BlockCopy(today, 0, full, 0, habitsCount * 4);
        Buffer.BlockCopy(sum7, 0, full, habitsCount * 4, habitsCount * 4);
        Buffer.BlockCopy(streak, 0, full, habitsCount * 8, habitsCount * 4);
        return full;
    }
}