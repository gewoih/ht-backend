using Microsoft.AspNetCore.Identity;

namespace HT.Domain.Entities.Identity;

public sealed class User : IdentityUser<Guid>
{
    public string Email { get; set; }
    public ICollection<Subscription> Subscriptions { get; set; }
    public ICollection<Habit>? Habits { get; set; }

    public Subscription CurrentSubscription => Subscriptions.Single(s => s.IsActive);
}