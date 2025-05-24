using HT.Core.Enums;

namespace HT.Core.Dto;

public record SubscriptionDto(SubscriptionType Type, DateTime StartDate, DateTime EndDate);