using Shared.Abstractions.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Services.Domain {
    public class Specialization : Entity<int> {
        public string Name { get; set; }
        public bool IsActive {  set; get; }
        public List<Service> Services { get; set; } = new();
    }
}
