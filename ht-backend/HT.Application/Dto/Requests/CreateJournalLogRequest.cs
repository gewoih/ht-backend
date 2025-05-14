using HT.Domain.ValueObjects;

namespace HT.Application.Dto.Requests;

public readonly record struct CreateJournalLogRequest(DateOnly Date, IEnumerable<HabitLogDto> HabitLogs, DailyScore DailyScore);