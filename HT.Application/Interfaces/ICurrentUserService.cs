using HT.Application.Dto;

namespace HT.Application.Interfaces;

public interface ICurrentUserService
{
    Guid GetUserId();
    Task<UserProfile?> GetUserProfileAsync(CancellationToken cancellationToken = default);
}