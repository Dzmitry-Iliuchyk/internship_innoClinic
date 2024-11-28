﻿using Authorization.Application.Dtos;
using FluentValidation;

namespace Authorization.Application.Validations {
    public class SignUpModelValidator: AbstractValidator<SignUpModel> {
        public SignUpModelValidator() {
            RuleFor( u => u.Email ).NotNull().EmailAddress();
            RuleFor( u => u.Password ).NotNull().MinimumLength( 6 ).MaximumLength( 15 );
        }

    }
}
