using HT.Application.Dto;
using HT.Domain.Entities.Identity;

namespace HT.Application.Mappers;

public static class UserMapper
{
    public static UserProfile ToUserProfile(this User user) =>
        new UserProfile(user.Email, user.CurrentSubscription.ToDto());
}