using HT.Application.Common.Interfaces;
using HT.Application.Journal.Dto;
using HT.Domain.Entities;
using HT.Domain.Repositories;
using MediatR;

namespace HT.Application.Journal.Commands;

public record CreateOrUpdateJournalLogCommand(CreateJournalLogRequest Data) : IRequest<Unit>;

public class CreateOrUpdateJournalLogHandler(
    IUserJournalRepository userJournalRepository,
    ICurrentUserService currentUserService)
    : IRequestHandler<CreateOrUpdateJournalLogCommand, Unit>
{
    public async Task<Unit> Handle(CreateOrUpdateJournalLogCommand request, CancellationToken cancellationToken)
    {
        var journalLogDateUtc = request.Data.Date.ToUniversalTime();
        var currentUserId = currentUserService.GetId();

        var existingLog =
            await userJournalRepository.GetByUserAndDateAsync(currentUserId, journalLogDateUtc, cancellationToken);

        if (existingLog != null)
        {
            existingLog.UpdateScores(request.Data.HealthScore, request.Data.EnergyScore, request.Data.MoodScore);

            existingLog.HabitLogs = request.Data.HabitLogs.Select(habitLog => new HabitLog
            {
                HabitId = habitLog.HabitId,
                Value = habitLog.Value ? 1.0f : 0.0f
            }).ToList();
        }
        else
        {
            var newLog = new JournalLog
            {
                UserId = request.Data.UserId,
                Date = journalLogDateUtc,
                HealthScore = request.Data.HealthScore,
                EnergyScore = request.Data.EnergyScore,
                MoodScore = request.Data.MoodScore,
                HabitLogs = request.Data.HabitLogs.Select(habitLog => new HabitLog
                {
                    HabitId = habitLog.HabitId,
                    Value = habitLog.Value ? 1.0f : 0.0f
                }).ToList()
            };

            await userJournalRepository.CreateAsync(newLog, cancellationToken);
        }
        
        return Unit.Value;
    }
}