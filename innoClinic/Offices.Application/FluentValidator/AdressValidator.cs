using FluentValidation;
using Offices.Domain.Models;

namespace Offices.Application.FluentValidator {
    public class AdressValidator: AbstractValidator<Address> {
        public AdressValidator() {
            RuleFor(a=>a.City).NotEmpty().NotNull();
            RuleFor(a=>a.Street).NotEmpty().NotNull();
            RuleFor( a => a.HouseNumber ).NotNull().NotEmpty().MaximumLength( 15 );
        }
    }
}
