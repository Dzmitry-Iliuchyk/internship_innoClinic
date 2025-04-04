﻿namespace Shared.Events.Contracts.ServiceMessages {
    public record SpecializationCreated {
        public int Id { get; init; }
        public required string Name { get; init; }
        public bool IsActive { get; init; }
    }
}

