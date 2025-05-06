namespace HT.Application.Dto;

public record CreateJournalLogRequest(
    Guid UserId,
    DateTime Date,
    IEnumerable<HabitLogDto> HabitLogs,
    int HealthScore,
    int EnergyScore,
    int MoodScore);