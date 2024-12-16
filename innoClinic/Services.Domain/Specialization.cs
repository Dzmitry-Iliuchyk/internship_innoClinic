using Shared.Abstractions.Entities;

namespace Services.Domain {
    public class Specialization : Entity<int> {
        public string Name { get; set; }
        public bool IsActive {  set; get; }
    }
}
