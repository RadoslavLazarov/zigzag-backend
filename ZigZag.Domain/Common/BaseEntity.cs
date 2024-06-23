using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ZigZag.Domain.Common
{
    public abstract class BaseEntity
    {
        [BsonId]
        public ObjectId Id { get; set; }
    }
}
