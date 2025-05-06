using HT.Application.Dto;

namespace HT.Application.Interfaces;

public interface IUserJournalService
{
    Task<List<JournalLogDto>> GetLogsAsync(CancellationToken cancellationToken = default);
    Task<JournalLogDto?> GetAsync(DateTime date, CancellationToken cancellationToken = default);
    Task CreateAsync(CreateJournalLogRequest request, CancellationToken cancellationToken = default);
}