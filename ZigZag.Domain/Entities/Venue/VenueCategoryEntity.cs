using HotChocolate.Types.MongoDb;
using HotChocolate.Types;
using ZigZag.Domain.Common;

namespace ZigZag.Domain.Entities.Venue
{
    public class VenueCategoryEntity : BaseEntity
    {
        public string Name { get; set; }
    }

    public class VenueCategoryEntityType : ObjectType<VenueCategoryEntity>
    {
        protected override void Configure(IObjectTypeDescriptor<VenueCategoryEntity> descriptor)
        {
            descriptor.Field(t => t.Id).Type<ObjectIdType>();
            descriptor.Field(t => t.Name);
        }
    }
}
