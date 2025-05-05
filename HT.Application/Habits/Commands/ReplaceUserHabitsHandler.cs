using HT.Application.Common.Interfaces;
using HT.Domain.UserHabits;
using HT.Domain.Users;
using MediatR;

namespace HT.Application.Habits.Commands.ReplaceUserHabits;

public record ReplaceUserHabitsCommand(IReadOnlyList<Guid> HabitIds) : IRequest<Unit>;

public class ReplaceUserHabitsHandler(
    ICurrentUserService currentUserService,
    IUserHabitRepository userHabitRepository, 
    IUserRepository userRepository) : IRequestHandler<ReplaceUserHabitsCommand, Unit>
{
    public async Task<Unit> Handle(ReplaceUserHabitsCommand request, CancellationToken cancellationToken)
    {
        //TODO: Вынести в отдельный валидатор
        var currentUserId = currentUserService.GetId();
        if (!await userRepository.ExistsAsync(currentUserId, cancellationToken))
            throw new Exception("User does not exist");

        await userHabitRepository.ReplaceAsync(currentUserId, request.HabitIds.ToList(), cancellationToken);
        return Unit.Value;
    }
}