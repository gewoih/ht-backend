using HT.Core.Entities.Base;

namespace HT.Core.Entities.Identity;

public class EmailConfirmationCode : Entity
{
    public Guid UserId { get; set; }
    public int Code { get; set; }
    public string Token { get; set; }
    public DateTime ExpiresAt { get; set; }
}