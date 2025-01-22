using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Offices.DataAccess.Models {
    public class OfficeEntity {
        [BsonId]
        [BsonRepresentation( BsonType.ObjectId )]
        public required string Id { get; set; }
        [BsonElement( "adrdess" )]
        [BsonRequired]
        public required AddressEntity Address { get; set; }
        [BsonElement( "registry_phone_number" )]
        [BsonRequired]
        public required string RegistryPhoneNumber { get; set; }
        [BsonElement( "status" )]
        [BsonRequired]
        public required bool Status { get; set; }
        [BsonElement( "phone_id" )]
        [BsonRequired]
        [BsonRepresentation( BsonType.String )]
        public string? PhotoUrl { get; set; }
    }
}
