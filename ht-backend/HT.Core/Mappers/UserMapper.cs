using HT.Core.Dto;
using HT.Core.Entities.Identity;

namespace HT.Core.Mappers;

public static class UserMapper
{
    public static UserProfile ToUserProfile(this User user) =>
        new(user.Email, user.UserName!, user.CurrentSubscription.ToDto());
}