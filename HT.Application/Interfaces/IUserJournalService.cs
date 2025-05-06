using HT.Application.Journal.Dto;

namespace HT.Application.Journal.Interfaces;

public interface IUserJournalService
{
    Task<List<JournalLogDto>> GetLogsAsync(CancellationToken cancellationToken = default);
    Task<JournalLogDto?> GetAsync(DateTime date, CancellationToken cancellationToken = default);
}