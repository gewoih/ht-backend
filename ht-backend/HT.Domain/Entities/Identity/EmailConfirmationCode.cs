using HT.Domain.Entities.Base;

namespace HT.Domain.Entities.Identity;

public class EmailConfirmationCode : Entity
{
    public Guid UserId { get; set; }
    public int Code { get; set; }
    public string Token { get; set; }
    public DateTime ExpiresAt { get; set; }
}