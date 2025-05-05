using HT.Application.Common.Interfaces;

namespace HT.Infrastructure.Auth;

public sealed class CurrentUserService : ICurrentUserService
{
    public Guid GetId() => Guid.Parse("01968ab7-bc43-762e-869b-ad7a8c5318a0");
}