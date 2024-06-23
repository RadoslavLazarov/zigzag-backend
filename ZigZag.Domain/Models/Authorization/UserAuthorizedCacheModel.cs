using MongoDB.Bson;

namespace ZigZag.Domain.Models.Authorization
{
    public class UserAuthorizedCacheModel
    {
        public string Id { get; set; }

        public string RefreshToken { get; set; }

        public string AccessToken { get; set; }
    }
}
