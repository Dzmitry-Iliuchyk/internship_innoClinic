namespace Offices.Application.Dtos {
    public class OfficeDto {
        public string Id { get; set; }
        public AddressDto Address { get; set; }
        public string RegistryPhoneNumber { get; set; }
        public bool Status { get; set; }
        public string? PhotoUrl { get; set; }
    }
}
