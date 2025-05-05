using HT.Application.Common.Exceptions;
using HT.Application.Habits.Dto;
using HT.Application.Habits.Interfaces;
using MediatR;

namespace HT.Application.Habits.Queries.GetHabitDetails;

public record GetHabitDetailsQuery(Guid HabitId) : IRequest<HabitDetailsDto>;

public class GetHabitDetailsHandler(IHabitService habitService) : IRequestHandler<GetHabitDetailsQuery, HabitDetailsDto>
{
    public async Task<HabitDetailsDto> Handle(GetHabitDetailsQuery request, CancellationToken cancellationToken)
    {
        var habitDetails = await habitService.GetDetailsAsync(request.HabitId, cancellationToken);
        if (habitDetails == null)
            throw new NotFoundException();

        return habitDetails;
    }
}