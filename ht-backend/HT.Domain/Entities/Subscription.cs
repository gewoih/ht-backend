using HT.Domain.Entities.Base;
using HT.Domain.Entities.Identity;
using HT.Domain.Enums;

namespace HT.Domain.Entities;

public class Subscription : Entity
{
    public Guid UserId { get; set; }
    public SubscriptionType Type { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsActive { get; set; }
    
    public User User { get; set; }
}