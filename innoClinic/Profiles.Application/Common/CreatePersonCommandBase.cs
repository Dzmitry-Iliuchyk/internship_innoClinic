using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Profiles.Application.Common {
    public abstract record CreatePersonCommandBase(
        [Required] string FirstName,
        [Required] string LastName,
        [Required] string Email,
        [Required] string PhoneNumber,
        Guid? CreatedBy,
        string? PhotoUrl,
        string? MiddleName
    ): IRequest<Guid>;

}

