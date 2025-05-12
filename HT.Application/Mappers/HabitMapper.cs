using HT.Application.Dto;
using HT.Domain.Entities;

namespace HT.Application.Mappers;

public static class HabitMapper
{
    public static HabitDto ToDto(this Habit habit) => new(habit.Id, habit.Category, habit.Name, habit.Impact);
    public static HabitDetailsDto ToDetailsDto(this Habit habit) => new(habit.Description, habit.Recommendation, habit.Impact);

    public static HabitLog ToDomain(this HabitLogDto habitLogDto) => new()
        { HabitId = habitLogDto.HabitId, Value = habitLogDto.Value };

    public static HabitLogDto ToDto(this HabitLog habitLog) => new(habitLog.HabitId, habitLog.Value);

    public static HabitDto ToDto(this UserHabit userHabit) =>
        new(userHabit.Habit!.Id, userHabit.Habit.Category, userHabit.Habit.Name, userHabit.Habit.Impact);
}