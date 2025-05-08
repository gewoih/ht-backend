using HT.Application.Dto;
using HT.Domain.Entities;

namespace HT.Application.Mappers;

public static class JournalMapper
{
    public static JournalLogDto ToDto(this JournalLog journalLog) => new()
    {
        Date = journalLog.Date,
        DailyScore = journalLog.Score,
        HabitLogs = journalLog.HabitLogs.Select(habitLog => habitLog.ToDto())
    };
}