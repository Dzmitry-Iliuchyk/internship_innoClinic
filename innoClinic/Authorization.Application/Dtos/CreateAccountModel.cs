using Authorization.Domain.Models.Enums;

namespace Authorization.Application.Dtos {
    public class CreateAccountModel {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Roles Roles { get; set; }
    }
}
