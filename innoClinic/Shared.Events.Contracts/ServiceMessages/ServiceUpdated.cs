namespace Shared.Events.Contracts.ServiceMessages {
    public record ServiceUpdated {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public required decimal Price { get; set; }
    }
}

