using Shared.Abstractions.Entities;
using System.Data;
using System.Text.Json.Serialization;

namespace Authorrization.Api.Models {
    public class User : Entity<Guid> {
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public ICollection<Role>? Roles { get; set; }
    }
}
