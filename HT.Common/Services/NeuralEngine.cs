using HT.Common.Dto;
using HT.Common.Dto.ML;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace HT.Common.Services;

public class NeuralEngine
{
    public Dictionary<Guid, double> GetHabitsImportance(List<JournalLogDto> journalLogs)
    {
        var habitIds = journalLogs
            .SelectMany(journalLog => journalLog.HabitLogs)
            .Select(habitLog => habitLog.HabitId)
            .OrderBy(habitLog => habitLog.ToString())
            .Distinct()
            .ToList();
        
        var avgScore = journalLogs.Average(j => j.Score);

        var mlContext = new MLContext(seed: 228);
        var data = journalLogs.Select(journalLog =>
        {
            var vector = new float[habitIds.Count];
            foreach (var habitLog in journalLog.HabitLogs)
            {
                var index = habitIds.FindIndex(h => h == habitLog.HabitId);
                if (index >= 0)
                    vector[index] = habitLog.Value ? 1.0f : -1.0f;
            }

            return new HabitData
            {
                Features = vector,
                Label = journalLog.Score - (float)avgScore
            };
        }).ToList();

        var schemaDefinition = SchemaDefinition.Create(typeof(HabitData));
        schemaDefinition["Features"].ColumnType = new VectorDataViewType(NumberDataViewType.Single, habitIds.Count);
        var dataView = mlContext.Data.LoadFromEnumerable(data, schemaDefinition);
        
        var pipeline = mlContext.Regression.Trainers.Sdca(maximumNumberOfIterations: 1000);
        var model = pipeline.Fit(dataView);

        var linearModel = model.Model;
        var weights = linearModel.Weights.ToArray();
        var result = new Dictionary<Guid, double>();
        for (var index = 0; index < habitIds.Count; index++)
        {
            var habitId = habitIds[index];
            result[habitId] = weights[index];
        }

        return result;
    }
}