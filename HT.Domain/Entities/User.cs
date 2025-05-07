using HT.Domain.Entities.Base;

namespace HT.Domain.Entities;

public sealed class User : NamedEntity
{
    public string Email { get; set; }
    public ICollection<Subscription> Subscriptions { get; set; }
    public ICollection<Habit>? Habits { get; set; }

    public Subscription CurrentSubscription => Subscriptions.Single(s => s.IsActive);
}