using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Offices.DataAccess.Models {
    public class OfficeEntity {
        [BsonId]
        [BsonRepresentation( BsonType.ObjectId )]
        public required string Id { get; set; }
        [BsonElement( "Adrdess" )]
        [BsonRequired]
        public required AddressEntity Address { get; set; }
        [BsonElement( "Registry_phone_number" )]
        [BsonRequired]
        public required string RegistryPhoneNumber { get; set; }
        [BsonElement( "Status" )]
        [BsonRequired]
        public required bool Status { get; set; }
        [BsonElement( "PhotoUrl" )]
        [BsonRequired]
        [BsonRepresentation( BsonType.String )]
        public string? PhotoUrl { get; set; }
    }
}
