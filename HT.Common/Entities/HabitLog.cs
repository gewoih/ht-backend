using HT.Common.Entities.Base;

namespace HT.Common.Entities;

public class HabitLog : Entity
{
    public Guid JournalLogId { get; set; }
    public Guid HabitId { get; set; }
    public float Value { get; set; }
}