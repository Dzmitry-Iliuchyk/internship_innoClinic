namespace Shared.Events.Contracts.ProfilesMessages {
    public record PatientDeleted {
        public Guid Id { get; init; }
    }
}