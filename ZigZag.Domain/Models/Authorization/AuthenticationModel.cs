namespace ZigZag.Domain.Models.Authorization
{
    public class AuthenticationModel
    {
        public string Email { get; set; }

        public List<string> Roles { get; set; }

        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }
    }
}
