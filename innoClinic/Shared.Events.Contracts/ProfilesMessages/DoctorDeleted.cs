namespace Shared.Events.Contracts.ProfilesMessages {
    public record DoctorDeleted {
        public Guid Id { get; init; }
    }
}