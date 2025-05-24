using Microsoft.AspNetCore.Identity;

namespace HT.Core.Entities.Identity;

public sealed class User : IdentityUser<Guid>
{
    public ICollection<Subscription> Subscriptions { get; set; }
    public ICollection<Habit>? Habits { get; set; }

    public Subscription CurrentSubscription => Subscriptions.Single(s => s.IsActive);
}