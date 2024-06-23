namespace ZigZag.Domain.Configurations
{
    public class JwtConfiguration
    {
        public string ValidAudience { get; set; }

        public string ValidIssuer { get; set; }

        public string Secret { get; set; }

        public int AccessTokenTTL { get; set; }

        public int RefreshTokenTTL { get; set; }
    }
}
