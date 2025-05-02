using HT.Common.Entities.Base;
using HT.Common.Enums;

namespace HT.Common.Entities;

public sealed class Habit : NamedEntity
{
    public string? Description { get; set; }
    public HabitPolarity Polarity { get; set; }
}