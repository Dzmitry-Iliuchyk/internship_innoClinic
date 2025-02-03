
namespace Shared.Events.Contracts {
    public record ProfileCreated {
        public Guid Id { get; init; }
        public required string Email { get; init; }
    }
    public record ProfileUpdated {
        public Guid Id { get; init; }
        public required string Email { get; init; }

    }
    public record ProfileDeleted {
        public Guid Id { get; init; }
    }
}