﻿
namespace Shared.Events.Contracts {
    public record SpecializationUpdated {
        public int Id { get; init; }
        public required string Name { get; init; }
        public bool IsActive { get; init; }
    }
}