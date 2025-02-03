﻿using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Profiles.Application.Common {
    public abstract record UpdatePersonCommandBase(
        [Required] Guid Id,
        [Required] string FirstName,
        [Required] string LastName,
        [Required] string Email,
        [Required] string PhoneNumber,
        [Required] Guid UpdatedBy,
        string? PhotoUrl,
        string? MiddleName
    ): IRequest;

}

