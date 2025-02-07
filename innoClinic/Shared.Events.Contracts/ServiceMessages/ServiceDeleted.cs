namespace Shared.Events.Contracts.ServiceMessages {
    public record ServiceDeleted {
        public required Guid Id { get; set; }
    }
}

