using HT.Core.Dto;
using HT.Core.Dto.Requests;

namespace HT.Core.Interfaces;

public interface IUserJournalService
{
    Task<List<JournalLogDto>> GetAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<JournalLogDto?> GetAsync(DateOnly date, CancellationToken cancellationToken = default);
    Task CreateAsync(CreateJournalLogRequest request, CancellationToken cancellationToken = default);
}