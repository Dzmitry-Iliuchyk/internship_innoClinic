using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Offices.DataAccess.Models {
    public class AddressEntity {
        [BsonElement( "city" )]
        [BsonRequired]
        public required string City { get; set; }
        [BsonElement( "street" )]
        [BsonRequired]
        public required string Street { get; set; }
        [BsonElement( "house_number" )]
        [BsonRequired]
        public required string HouseNumber { get; set; }
        [BsonElement( "office_number" )]
        public string? OfficeNumber { get; set; }
    }
}
