namespace HT.Application.Dto.Requests;

public record CreateJournalLogRequest(
    Guid UserId,
    DateOnly Date,
    IEnumerable<HabitLogDto> HabitLogs,
    int HealthScore,
    int EnergyScore,
    int MoodScore);