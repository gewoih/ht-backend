using HT.Application.Habits.Dto;

namespace HT.Application.Journal.Dto;

public record CreateJournalLogRequest(
    Guid UserId,
    DateTime Date,
    IEnumerable<HabitLogDto> HabitLogs,
    int HealthScore,
    int EnergyScore,
    int MoodScore);