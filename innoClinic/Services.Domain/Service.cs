using Shared.Abstractions.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Services.Domain {
    public class Service : Entity<Guid> {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public ServiceCategory? Category { get; set; }
        public int SpecializationId { get; set; }
        public Specialization? Specialization { get; set; }
        public bool IsActive { get; set; }

    }
}
