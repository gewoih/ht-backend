namespace HT.Common.Dto;

public record JournalLogDto(int Score, IEnumerable<HabitLogDto> HabitLogs);