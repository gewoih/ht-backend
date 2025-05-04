using HT.Common.Entities.Base;

namespace HT.Common.Entities;

public class UserHabit : Entity
{
    public Guid UserId { get; set; }
    public User? User { get; set; }
    public Guid HabitId { get; set; }
    public Habit? Habit { get; set; }
}