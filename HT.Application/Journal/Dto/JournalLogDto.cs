using System.Text.Json.Serialization;
using HT.Application.Habits.Dto;

namespace HT.Application.Journal.Dto;

public class JournalLogDto(
    DateTime date,
    int healthScore,
    int energyScore,
    int moodScore,
    IEnumerable<HabitLogDto> habitLogs)
{
    public DateTime Date { get; init; } = date;
    public int HealthScore { get; init; } = healthScore;
    public int EnergyScore { get; init; } = energyScore;
    public int MoodScore { get; init; } = moodScore;

    [JsonIgnore] public int Score => HealthScore + EnergyScore + MoodScore;
    public IEnumerable<HabitLogDto> HabitLogs { get; init; } = habitLogs;
}