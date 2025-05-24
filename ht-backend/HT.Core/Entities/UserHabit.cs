using HT.Core.Entities.Base;
using HT.Core.Entities.Identity;

namespace HT.Core.Entities;

public class UserHabit : Entity
{
    public Guid UserId { get; set; }
    public User? User { get; set; }
    public Guid HabitId { get; set; }
    public Habit? Habit { get; set; }
}