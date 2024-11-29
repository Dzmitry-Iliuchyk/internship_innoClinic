using Authorrization.Api.Models;
using FluentValidation;

namespace Authorization.Application.Validations {
    public class UserValidator: AbstractValidator<User> {
        public UserValidator() {
            RuleFor( u => u.Email ).NotNull().EmailAddress();
        }

    }
}
