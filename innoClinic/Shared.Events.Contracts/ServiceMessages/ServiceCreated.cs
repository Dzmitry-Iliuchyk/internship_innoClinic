namespace Shared.Events.Contracts.ServiceMessages {
    public record ServiceCreated {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public required decimal Price { get; set; }
    }
}

