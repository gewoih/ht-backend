using HT.Core.Dto;

namespace HT.Core.Interfaces;

public interface ICurrentUserService
{
    Guid GetUserId();
    Task<UserProfile?> GetUserProfileAsync(CancellationToken cancellationToken = default);
}