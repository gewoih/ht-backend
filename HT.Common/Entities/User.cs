using HT.Common.Entities.Base;

namespace HT.Common.Entities;

public sealed class User : NamedEntity
{
    public ICollection<Habit>? Habits { get; set; }
}