using HT.Domain.Enums;

namespace HT.Application.Dto;

public record SubscriptionDto(SubscriptionType Type, DateTime StartDate, DateTime EndDate);