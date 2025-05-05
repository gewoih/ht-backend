using HT.Domain.Entities.Base;
using HT.Domain.Habits;

namespace HT.Domain.Users;

public sealed class User : NamedEntity
{
    public ICollection<Habit>? Habits { get; set; }
}