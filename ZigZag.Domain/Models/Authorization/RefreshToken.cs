namespace ZigZag.Domain.Models.Authorization
{
    public class RefreshTokenModel
    {
        public string Token { get; set; }

        public DateTime Expires { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? Revoked { get; set; }

        public bool IsExpired => DateTime.UtcNow >= Expires;

        public bool IsRevoked => Revoked != null;

        public bool IsActive => !IsRevoked && !IsExpired;
    }
}
