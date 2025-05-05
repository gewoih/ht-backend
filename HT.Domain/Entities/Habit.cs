using System.ComponentModel.DataAnnotations;
using HT.Domain.Entities.Base;
using HT.Domain.Users;

namespace HT.Domain.Habits;

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