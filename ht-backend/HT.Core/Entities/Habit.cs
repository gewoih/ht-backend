using System.ComponentModel.DataAnnotations;
using HT.Core.Entities.Base;
using HT.Core.Entities.Identity;

namespace HT.Core.Entities;

public sealed class Habit : NamedEntity
{
    [Required]
    public int Impact { get; set; }
    
    [Required]
    public string Category { get; set; }
    
    [Required]
    public string Description { get; set; }
    
    [Required]
    public string Recommendation { get; set; }

    public ICollection<User>? Users { get; set; }
}