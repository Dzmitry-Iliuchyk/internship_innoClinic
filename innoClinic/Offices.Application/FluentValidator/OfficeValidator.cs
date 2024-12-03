using FluentValidation;
using Offices.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Offices.Application.FluentValidator {
    public class OfficeValidator: AbstractValidator<Office> {
        public OfficeValidator() {
            RuleFor( o => o.Id ).NotEmpty().NotNull();
            RuleFor( o => o.Address ).NotNull().SetValidator( new AdressValidator() );
            RuleFor( o => o.RegistryPhoneNumber ).NotEmpty().NotNull().MinimumLength(6).MaximumLength(21);
        }
    }
}
