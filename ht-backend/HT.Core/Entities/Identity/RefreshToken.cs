using HT.Core.Entities.Base;

namespace HT.Core.Entities.Identity;

public class RefreshToken : Entity
{
    public Guid UserId { get; set; }
    public User User { get; set; }
    public required byte[] Hash { get; set; }
    public DateTime ExpiresAt { get; set; }
    public DateTime? RevokedAt { get; set; }
    public Guid? ReplacedByTokenId { get; set; }
}