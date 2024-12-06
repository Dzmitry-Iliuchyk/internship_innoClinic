namespace Offices.Domain.Models {
    public class Office {
        public string Id { get; set; }
        public Address Address { get; set; }
        public string RegistryPhoneNumber { get; set; }
        public bool Status { get; set; }
        public Guid? PhotoId { get; set; }
    }
}
