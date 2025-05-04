using System.ComponentModel.DataAnnotations;
using HT.Common.Entities.Base;

namespace HT.Common.Entities;

public sealed class Habit : NamedEntity
{
    [Required]
    public string Category { get; set; }
    
    [Required]
    public string Description { get; set; }
    
    [Required]
    public string Recommendation { get; set; }

    public ICollection<User>? Users { get; set; }
}