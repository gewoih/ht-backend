using HT.Domain.Entities;
using HT.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace HT.Infrastructure.Persistence.Users;

public class UserRepository(HtContext context) : IUserRepository
{
    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Users.AnyAsync(user => user.Id == id, cancellationToken);
    }
}