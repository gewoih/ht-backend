using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using HT.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HT.Infrastructure.Persistence;

public class DatabaseInitializer(HtContext context)
{
    private readonly string _habitsCsvFilePath = Path.Combine(AppContext.BaseDirectory, "data/habits.csv");
    
    public async Task SeedAsync()
    {
        if (await context.Habits.AnyAsync())
            return;
        
        var habits = LoadHabitsFromCsv(_habitsCsvFilePath);
        
        await context.Habits.AddRangeAsync(habits);
        await context.SaveChangesAsync();
    }
    
    private static List<Habit> LoadHabitsFromCsv(string filePath)
    {
        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = ";",
        });

        var records = csv.GetRecords<HabitCsvModel>().ToList();

        return records.Select(habitModel => new Habit
        {
            Name = habitModel.Name,
            Category = habitModel.Category,
            Description = habitModel.Description,
            Recommendation = habitModel.Recommendation,
            Impact = int.Parse(habitModel.Impact)
        }).ToList();
    }

    private class HabitCsvModel
    {
        public string Name { get; set; } = "";
        public string Category { get; set; } = "";
        public string Description { get; set; } = "";
        public string Recommendation { get; set; } = "";
        public string Impact { get; set; } = "";
    }
}