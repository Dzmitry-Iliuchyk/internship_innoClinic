using System.Data;
using System.Text.Json.Serialization;

namespace Authorrization.Api.Models {
    public class User {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public ICollection<Role>? Roles { get; set; }
    }
}
