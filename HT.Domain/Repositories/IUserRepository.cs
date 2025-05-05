namespace HT.Domain.Users;

public interface IUserRepository
{
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
}