using HT.Domain.ValueObjects;

namespace HT.Application.Dto;

public readonly record struct DateScoreDto(DateOnly Date, DailyScore Score);