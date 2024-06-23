namespace ZigZag.Domain.Models.Authorization
{
    public class TokenResponseModel
    {
        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }
    }
}
