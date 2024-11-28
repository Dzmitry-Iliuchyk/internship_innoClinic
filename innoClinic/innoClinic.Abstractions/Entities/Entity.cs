using System;

namespace Shared.Abstractions.Entities {
    public abstract class Entity<TIdentifier> {
        public TIdentifier Id { get; set; }

    }
}
