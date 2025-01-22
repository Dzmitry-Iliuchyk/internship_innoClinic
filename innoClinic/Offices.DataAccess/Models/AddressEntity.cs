using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Offices.DataAccess.Models {
    public class AddressEntity {
        [BsonElement( "City" )]
        [BsonRequired]
        public required string City { get; set; }
        [BsonElement( "Street" )]
        [BsonRequired]
        public required string Street { get; set; }
        [BsonElement( "House_number" )]
        [BsonRequired]
        public required string HouseNumber { get; set; }
        [BsonElement( "Office_number" )]
        public string? OfficeNumber { get; set; }
    }
}
