using HT.Application.Habits.Dto;
using HT.Application.Habits.Interfaces;
using MediatR;

namespace HT.Application.Habits.Queries.GetHabitIds;

public record GetAllHabitIdsQuery : IRequest<IReadOnlyList<HabitDto>>;

public class GetAllHabitIdsHandler(IHabitService habitService) : IRequestHandler<GetAllHabitIdsQuery, IReadOnlyList<HabitDto>>
{
    public async Task<IReadOnlyList<HabitDto>> Handle(GetAllHabitIdsQuery request, CancellationToken cancellationToken)
    {
        return await habitService.GetAsync(cancellationToken);
    }
}