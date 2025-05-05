using HT.Application.Habits.Dto;
using HT.Application.Habits.Interfaces;
using MediatR;

namespace HT.Application.Habits.Queries.GetAllHabits;

public record GetAllHabitsQuery : IRequest<IReadOnlyList<HabitDto>>;

public class GetAllHabitsHandler(IUserHabitService userHabitService)
    : IRequestHandler<GetAllHabitsQuery, IReadOnlyList<HabitDto>>
{
    public async Task<IReadOnlyList<HabitDto>> Handle(GetAllHabitsQuery _, CancellationToken cancellationToken)
    {
        return await userHabitService.GetAsync(cancellationToken);
    }
}