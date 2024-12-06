using FluentValidation;
using Offices.Domain.Models;

namespace Offices.Application.FluentValidator {
    public class OfficeValidator: AbstractValidator<Office> {
        public OfficeValidator() {
            RuleFor( o => o.Id ).NotEmpty().NotNull();
            RuleFor( o => o.Address ).NotNull().SetValidator( new AdressValidator() );
            RuleFor( o => o.RegistryPhoneNumber )
                .NotEmpty()
                .NotNull()
                .Matches( @"^\+?\d{10,15}$" )
                .WithMessage( "Invalid phone number format. Phone number must be between 10 and 15 digits and can start with a '+' sign." );
        }
    }
}
