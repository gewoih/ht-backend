using System.ComponentModel.DataAnnotations;
using HT.Common.Entities.Base;
using HT.Common.Enums;

namespace HT.Common.Entities;

public sealed class Habit : NamedEntity
{
    [Required]
    public string Category { get; set; }
    public string? Description { get; set; }
    public string? Recommendation { get; set; }
    public HabitPolarity Polarity { get; set; }
}