using HotChocolate.Types;
using HotChocolate.Types.MongoDb;
using MongoDB.Bson;
using ZigZag.Domain.Common;

namespace ZigZag.Domain.Entities.Venue
{
    public class VenueEntity : BaseEntity
    {
        public string Name { get; set; }

        public ObjectId VenueCategoryId { get; set; }
    }

    public class VenueEntityType : ObjectType<VenueEntity>
    {
        protected override void Configure(IObjectTypeDescriptor<VenueEntity> descriptor)
        {
            descriptor.Field(t => t.Id).Type<ObjectIdType>();
            descriptor.Field(t => t.Name);
            descriptor.Field(t => t.VenueCategoryId).Type<ObjectIdType>();
        }
    }
}
