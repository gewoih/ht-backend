namespace HT.Common.Dto;

public record JournalLogDto(DateTime Date, int Score, IEnumerable<HabitLogDto> HabitLogs);