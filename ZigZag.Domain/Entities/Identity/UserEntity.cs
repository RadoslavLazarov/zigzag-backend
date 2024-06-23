using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;
using ZigZag.Domain.Models.Authorization;

namespace ZigZag.Domain.Entities.Identity
{
    public class UserEntity : IdentityUser<ObjectId>
    {
        public bool IsSystem { get; set; }

        public List<string> Roles { get; set; } = new();
    }
}