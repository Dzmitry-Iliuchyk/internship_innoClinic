using Shared.Abstractions.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Services.Domain {
    public class ServiceCategory : Entity<int> {
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public TimeSpan TimeSlotSize { get; set; }
        public List<Service> Services { get; set; } = new();
    }
}
