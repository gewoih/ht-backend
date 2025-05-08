using System.Text.Json.Serialization;

namespace HT.Application.Dto;

public class JournalLogDto
{
    public DateTime Date { get; set; }
    public int HealthScore { get; set; }
    public int EnergyScore { get; set; }
    public int MoodScore { get; set; }

    [JsonIgnore] public int Score => HealthScore + EnergyScore + MoodScore;
    public IEnumerable<HabitLogDto> HabitLogs { get; set; }
    
    public void UpdateHabitLogs(IEnumerable<HabitLogDto> habitLogs) => HabitLogs = habitLogs;
}