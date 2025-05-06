using HT.Domain.Entities.Base;

namespace HT.Domain.Entities;

public sealed class User : NamedEntity
{
    public ICollection<Habit>? Habits { get; set; }
}