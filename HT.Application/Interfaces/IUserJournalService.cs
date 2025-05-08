using HT.Application.Dto;
using HT.Application.Dto.Requests;

namespace HT.Application.Interfaces;

public interface IUserJournalService
{
    Task<List<JournalLogDto>> GetLogsAsync(CancellationToken cancellationToken = default);
    Task<JournalLogDto?> GetAsync(DateOnly date, CancellationToken cancellationToken = default);
    Task CreateAsync(CreateJournalLogRequest request, CancellationToken cancellationToken = default);
}