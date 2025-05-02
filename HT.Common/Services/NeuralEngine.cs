using HT.Common.Dto;
using HT.Common.Dto.ML;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace HT.Common.Services;

public class NeuralEngine
{
    public void GetHabitsImportance(List<JournalLogDto> journalLogs)
    {
        var habits = journalLogs.SelectMany(journalLog => journalLog.HabitLogs).Distinct().ToList();

        var mlContext = new MLContext(seed: 228);
        var data = journalLogs.Select(journalLog =>
        {
            var vector = new float[habits.Count];
            foreach (var habitLog in journalLog.HabitLogs)
            {
                var index = habits.FindIndex(h => h.Id == habitLog.HabitId);
                if (index >= 0)
                    vector[index] = habitLog.Value ? 1.0f : 0.0f;
            }

            return new HabitData
            {
                Features = vector,
                Label = journalLog.Score
            };
        }).ToList();

        var schemaDefinition = SchemaDefinition.Create(typeof(HabitData));
        schemaDefinition["Features"].ColumnType = new VectorDataViewType(NumberDataViewType.Single, habits.Count);
        var dataView = mlContext.Data.LoadFromEnumerable(data, schemaDefinition);
        
        var pipeline = mlContext.Regression.Trainers.Sdca();
        var model = pipeline.Fit(dataView);

        var transformedData = model.Transform(dataView);
        var permutationMetrics = mlContext.Regression.PermutationFeatureImportance(
            model, transformedData, labelColumnName: "Label");

        var linearModel = model.Model;
        var weights = linearModel.Weights.ToArray();
        var result = new Dictionary<string, (double importance, float weight)>();
        for (var i = 0; i < habitNames.Length; i++)
        {
            result[habitNames[i]] = (
                importance: Math.Abs(permutationMetrics[i].RSquared.Mean) * 100,
                weight: weights[i]
            );
        }

        var sorted = result
            .OrderByDescending(kv => kv.Value.importance * kv.Value.weight)
            .ToList();
    }
}