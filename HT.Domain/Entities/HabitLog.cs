using System.ComponentModel.DataAnnotations;
using HT.Domain.Entities.Base;

namespace HT.Domain.Entities;

public class HabitLog : Entity
{
    public Guid JournalLogId { get; set; }
    public virtual JournalLog JournalLog { get; set; }
    
    public Guid HabitId { get; set; }
    public virtual Habit Habit { get; set; }
    
    public bool Value { get; set; }
}