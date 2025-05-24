using HT.Core.Entities.Base;
using HT.Core.Entities.Identity;
using HT.Core.Enums;

namespace HT.Core.Entities;

public class Subscription : Entity
{
    public Guid UserId { get; set; }
    public SubscriptionType Type { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsActive { get; set; }
    
    public User User { get; set; }
}