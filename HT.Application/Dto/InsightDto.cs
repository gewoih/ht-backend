using HT.Application.Habits.Dto;

namespace HT.Application.Insights.Dto;

public record InsightDto(HabitDto Habit, double Influence);