using Shared.Abstractions.Entities;

namespace Authorrization.Api.Models {
    public class Role : Entity<int> {
        public string Name { get; set; }
    }
}
