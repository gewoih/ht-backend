using HT.Core.ValueObjects;

namespace HT.Core.Dto.Requests;

public readonly record struct CreateJournalLogRequest(DateOnly Date, IEnumerable<HabitLogDto> HabitLogs, DailyScore DailyScore);