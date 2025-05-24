using HT.Core.Dto;
using HT.Core.Entities;

namespace HT.Core.Mappers;

public static class SubscriptionMapper
{
    public static SubscriptionDto ToDto(this Subscription subscription) =>
        new(subscription.Type, subscription.StartDate, subscription.EndDate);
}