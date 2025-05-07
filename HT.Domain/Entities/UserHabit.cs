using HT.Domain.Entities.Base;
using HT.Domain.Entities.Identity;

namespace HT.Domain.Entities;

public class UserHabit : Entity
{
    public Guid UserId { get; set; }
    public User? User { get; set; }
    public Guid HabitId { get; set; }
    public Habit? Habit { get; set; }
}