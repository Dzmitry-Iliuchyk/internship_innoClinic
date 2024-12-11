using Shared.Abstractions.Entities;

namespace Profiles.Domain {
    public class Specialization: Entity<int> {
        public string Name { get; set; } = null!;
        public bool isActive { get; set; }
    }
}
