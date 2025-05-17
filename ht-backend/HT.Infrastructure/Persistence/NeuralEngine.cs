using Microsoft.ML;
using Microsoft.ML.Data;
using HT.Application.Dto;

namespace HT.Infrastructure.Persistence;

public sealed class NeuralEngine
{
    private readonly MLContext _ml = new(seed: 228);

    public Dictionary<Guid, double> GetHabitsImportance(IReadOnlyCollection<JournalLogDto> logs)
    {
        if (logs is null) throw new ArgumentNullException(nameof(logs));
        if (logs.Count == 0) return new Dictionary<Guid, double>();

        /*---------- подготовка данных ----------*/
        var habitIds = GetHabitIds(logs);
        var id2Index = habitIds
            .Select((id, idx) => (id, idx))
            .ToDictionary(t => t.id, t => t.idx);

        var rows = BuildRows(logs, habitIds.Count, id2Index);

        var schema = SchemaDefinition.Create(typeof(HabitData));
        schema[nameof(HabitData.Features)]
            .ColumnType = new VectorDataViewType(NumberDataViewType.Single, habitIds.Count);

        var data = _ml.Data.LoadFromEnumerable(rows, schema);

        /*---------- обучение модели ----------*/
        var pipeline = _ml
            .Transforms.NormalizeMeanVariance(nameof(HabitData.Features))
            .Append(_ml.Regression.Trainers.Sdca(
                labelColumnName: nameof(HabitData.Label),
                featureColumnName: nameof(HabitData.Features),
                l2Regularization: 0.1f,
                maximumNumberOfIterations: 1_000));

        var model = pipeline.Fit(data);

        /*---------- извлечение и нормализация весов ----------*/
        var weights = model.LastTransformer.Model.Weights.ToArray();
        var totalAbs = weights.Select(Math.Abs).Sum();

        var importance = new Dictionary<Guid, double>(habitIds.Count);
        for (var i = 0; i < habitIds.Count; i++)
        {
            var percent = totalAbs == 0 ? 0 : Math.Round(weights[i] / totalAbs * 100.0, 2);
            importance[habitIds[i]] = percent;
        }

        return importance;
    }

    private static List<Guid> GetHabitIds(IEnumerable<JournalLogDto> logs) =>
        logs.SelectMany(j => j.HabitLogs)
            .Select(h => h.HabitId)
            .Distinct()
            .OrderBy(id => id)
            .ToList();

    private static IEnumerable<HabitData> BuildRows(
        IEnumerable<JournalLogDto> logs,
        int featureCount,
        Dictionary<Guid, int> id2Index)
    {
        foreach (var log in logs.OrderBy(l => l.Date))
        {
            var features = new float[featureCount];
            foreach (var hl in log.HabitLogs)
                features[id2Index[hl.HabitId]] = hl.Value ? 1f : -1f;

            yield return new HabitData
            {
                Features = features,
                Label = log.DailyScore.Total
            };
        }
    }
}