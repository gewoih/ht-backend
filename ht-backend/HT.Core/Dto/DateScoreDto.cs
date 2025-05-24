using HT.Core.ValueObjects;

namespace HT.Core.Dto;

public readonly record struct DateScoreDto(DateOnly Date, DailyScore Score);