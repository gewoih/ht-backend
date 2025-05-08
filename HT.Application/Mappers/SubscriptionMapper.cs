using HT.Application.Dto;
using HT.Domain.Entities;

namespace HT.Application.Mappers;

public static class SubscriptionMapper
{
    public static SubscriptionDto ToDto(this Subscription subscription) =>
        new(subscription.Type, subscription.StartDate, subscription.EndDate);
}