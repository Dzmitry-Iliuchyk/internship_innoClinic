
namespace Shared.Events.Contracts {
    public record ProfileCreated {
        public Guid Id { get; init; }
        public required string Email { get; init; }
        public required Roles Roles { get; init; }
    }
    public record ProfileUpdated {
        public Guid Id { get; init; }
        public required string Email { get; init; }
    }
    public record ProfileDeleted {
        public Guid Id { get; init; }
    }
    public enum Roles {
        Admin = 1,
        Patient = 2,
        Doctor = 3,
        Receptionist = 4
    }
}