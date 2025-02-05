namespace Shared.Events.Contracts.ProfilesMessages {
    public record DoctorUpdated {
        public Guid Id { get; init; }
        public required string FirstName { get; init; }
        public required string SecondName { get; init; }
        public required string Email { get; init; }
        public required string Specialization { get; init; }
    }
}