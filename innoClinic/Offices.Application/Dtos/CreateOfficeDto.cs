namespace Offices.Application.Dtos {
    public class CreateOfficeDto {
        public AddressDto Address { get; set; }
        public string RegistryPhoneNumber { get; set; }
        public bool Status { get; set; }
        public Guid? PhotoId { get; set; }
    }
}
