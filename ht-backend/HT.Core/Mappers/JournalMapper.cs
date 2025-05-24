using HT.Core.Dto;
using HT.Core.Entities;

namespace HT.Core.Mappers;

public static class JournalMapper
{
    public static JournalLogDto ToDto(this JournalLog journalLog) => new()
    {
        Date = journalLog.Date,
        DailyScore = journalLog.Score,
        HabitLogs = journalLog.HabitLogs is not null ? journalLog.HabitLogs.Select(habitLog => habitLog.ToDto()) : []
    };
}