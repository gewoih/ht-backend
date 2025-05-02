using HT.Common.Entities.Base;

namespace HT.Common.Entities;

public class JournalLog : Entity
{
    public Guid UserId { get; set; }
    public string? Description { get; set; }
    public DateTime Date { get; set; }
    public ICollection<HabitLog> HabitLogs { get; set; }
    public int Score { get; set; }
}