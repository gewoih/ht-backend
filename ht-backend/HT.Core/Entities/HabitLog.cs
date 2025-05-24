using HT.Core.Entities.Base;

namespace HT.Core.Entities;

public class HabitLog : Entity
{
    public Guid JournalLogId { get; set; }
    public virtual JournalLog JournalLog { get; set; }
    
    public Guid HabitId { get; set; }
    public virtual Habit Habit { get; set; }
    
    public bool Value { get; set; }
}