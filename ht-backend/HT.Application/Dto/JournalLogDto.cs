using HT.Domain.ValueObjects;

namespace HT.Application.Dto;

public class JournalLogDto
{
    public DateOnly Date { get; set; }
    public DailyScore DailyScore { get; set; }
    public IEnumerable<HabitLogDto> HabitLogs { get; set; }
    
    public void UpdateHabitLogs(IEnumerable<HabitLogDto> habitLogs) => HabitLogs = habitLogs;
}