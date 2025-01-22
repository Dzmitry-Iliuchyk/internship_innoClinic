namespace FacadeApi.Offices.Dtos {
    public class UpdateOfficeDto {
        public string Id { get; set; }
        public AddressDto Address { get; set; }
        public string RegistryPhoneNumber { get; set; }
        public bool Status { get; set; }
        public Photo? Photo { get; set; }
    }
    public class UpdateOfficeDtoForApi {
        public AddressDto Address { get; set; }
        public string RegistryPhoneNumber { get; set; }
        public bool Status { get; set; }
        public string? PhotoUrl { get; set; }
    }
    public class CreateOfficeDto {
        public AddressDto Address { get; set; }
        public string RegistryPhoneNumber { get; set; }
        public bool Status { get; set; }
        public Photo? Photo { get; set; }
    }
    public class CreateOfficeDtoForApi {
        public AddressDto Address { get; set; }
        public string RegistryPhoneNumber { get; set; }
        public bool Status { get; set; }
        public string? PhotoUrl { get; set; }
    }
    public class OfficeDtoFromApi {
        public string Id { get; set; }
        public AddressDto Address { get; set; }
        public string RegistryPhoneNumber { get; set; }
        public bool Status { get; set; }
        public string? PhotoUrl { get; set; }
    }
    public class OfficeDto {
        public string Id { get; set; }
        public AddressDto Address { get; set; }
        public string RegistryPhoneNumber { get; set; }
        public bool Status { get; set; }
        public Photo? Photo { get; set; }
    }
    public class AddressDto {
        public string City { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public string? OfficeNumber { get; set; }
    }
    public class Photo {
        public string Name { get; set; }
        public byte[] Content { get; set; }
        public IDictionary<string, string>? Metadata { get; set; }
    }
}
