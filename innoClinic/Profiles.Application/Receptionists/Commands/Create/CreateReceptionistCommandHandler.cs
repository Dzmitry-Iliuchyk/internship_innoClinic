using Mapster;
using MediatR;
using Profiles.Application.Common;
using Profiles.Application.Interfaces.Repositories;
using Profiles.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profiles.Application.Receptionists.Commands.Create {
    public record CreateReceptionistCommand(
        [Required] string OfficeId,
        [Required] string FirstName,
        [Required] string LastName,
        [Required] string Email,
        [Required] string PhoneNumber,
        [Required] Guid CreatedBy,
        string? PhotoUrl,
        string? MiddleName ): CreatePersonCommandBase( FirstName, LastName, Email, PhoneNumber, CreatedBy, PhotoUrl, MiddleName );

    public class CreateReceptionistCommandHandler: IRequestHandler<CreateReceptionistCommand, Guid> {
        private readonly IReceptionistCommandRepository _repository;
        public CreateReceptionistCommandHandler( IReceptionistCommandRepository repository ) {
            this._repository = repository;
        }

        public async Task<Guid> Handle( CreateReceptionistCommand request, CancellationToken cancellationToken = default ) {
            var receptionist = request.Adapt<Receptionist>();
            await _repository.CreateAsync( receptionist );
            return receptionist.Id;
        }
    }
}
