using MongoDB.Bson;
using ZigZag.Domain.Common;

namespace ZigZag.Domain.Models.Venue
{
    public class VenueModel : BaseModel
    {
        public string Name { get; set; }

        public ObjectId VenueCategoryId { get; set; }
    }
}
