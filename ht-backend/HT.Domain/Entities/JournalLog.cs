using HT.Domain.Entities.Base;
using HT.Domain.ValueObjects;

namespace HT.Domain.Entities;

public class JournalLog : Entity
{
    public JournalLog() { }

    public JournalLog(Guid userId, DateOnly date, DailyScore score, ICollection<HabitLog> habitLogs)
    {
        UserId = userId;
        Date = date;
        Score = score;
        HabitLogs = habitLogs;
    }

    public Guid UserId { get; set; }
    public string? Description { get; set; }
    public DateOnly Date { get; set; }
    public ICollection<HabitLog> HabitLogs { get; set; }
    public DailyScore Score { get; set; }
}