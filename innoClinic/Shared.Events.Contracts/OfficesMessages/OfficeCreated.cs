namespace Shared.Events.Contracts.OfficesMessages {
    public record OfficeCreated {
        public required string Id { get; set; }
        public required string Address { get; set; }
        public required string RegistryPhoneNumber { get; set; }
        public required bool Status { get; set; }
    }
}

