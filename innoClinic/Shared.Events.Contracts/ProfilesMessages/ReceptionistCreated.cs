namespace Shared.Events.Contracts.ProfilesMessages {
    public record ReceptionistCreated {
        public Guid Id { get; init; }
        public required string FirstName { get; init; }
        public required string SecondName { get; init; }
        public required string Email { get; init; }
    }
}