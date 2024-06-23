using MongoDB.Bson;

namespace ZigZag.Domain.Models.Authorization
{
    public class CurrentUserModel
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public IEnumerable<string> Roles { get; set; } = new HashSet<string>();
    }
}
