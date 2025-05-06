using System.ComponentModel.DataAnnotations;
using HT.Domain.Entities.Base;

namespace HT.Domain.Entities;

public class HabitLog : Entity
{
    public Guid JournalLogId { get; set; }
    
    public Guid HabitId { get; set; }
    
    [Required]
    public Habit Habit { get; set; }
    
    public float Value { get; set; }
}