using HT.Application.Common.Exceptions;
using HT.Application.Journal.Dto;
using HT.Application.Journal.Interfaces;
using MediatR;

namespace HT.Application.Journal.Queries;

public record GetUserJournalQuery(DateTime Date) : IRequest<JournalLogDto?>;

public class GetUserJournalHandler(IUserJournalService userJournalService) : IRequestHandler<GetUserJournalQuery, JournalLogDto?>
{
    public async Task<JournalLogDto?> Handle(GetUserJournalQuery request, CancellationToken cancellationToken)
    {
        var journalLog = await userJournalService.GetAsync(request.Date, cancellationToken);
        if (journalLog == null)
            throw new NotFoundException();
        
        return journalLog;
    }
}