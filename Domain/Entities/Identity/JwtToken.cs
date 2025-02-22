using Domain.Commons;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Identity
{
    public class JwtToken : BaseEntity
    {
        public string UserId { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTime TokenExpirationDate { get; set; }
        public DateTime RefreshTokenExpirationDate { get; set; }
        public DateTime? RefreshTokenRevokedDate { get; set; }
        public bool IsRefreshTokenUsed { get; set; }
        public string? DeviceInfo { get; set; }
        public string? IPAddress { get; set; }

        public bool IsRefreshTokenExpired => DateTime.UtcNow >= RefreshTokenExpirationDate;
        public bool IsRefreshTokenActive => RefreshTokenRevokedDate == null && !IsRefreshTokenExpired;

        [ForeignKey(nameof(UserId))]
        [InverseProperty(nameof(User.JwtTokens))]
        public virtual User? User { get; set; }
    }
}
