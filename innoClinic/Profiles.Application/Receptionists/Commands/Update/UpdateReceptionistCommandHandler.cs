using Mapster;
using MediatR;
using Profiles.Application.Common;
using Profiles.Application.Common.Exceptions;
using Profiles.Application.Interfaces.Repositories;
using Profiles.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profiles.Application.Receptionists.Commands.Update {
    public record UpdateReceptionistCommand([Required] Guid Id,
                                            [Required] string OfficeId,
                                            [Required] string FirstName,
                                            [Required] string LastName,
                                            [Required] string Email,
                                            [Required] string PhoneNumber,
                                            [Required] Guid UpdatedBy,
                                            string? PhotoUrl,
                                            string? MiddleName ): UpdatePersonCommandBase( Id, FirstName, LastName, Email, PhoneNumber, UpdatedBy, PhotoUrl, MiddleName );

    public class UpdateReceptionistCommandHandler: IRequestHandler<UpdateReceptionistCommand> {
        private readonly IReceptionistCommandRepository _repository;
        private readonly IReceptionistReadRepository _repoRead;
        public UpdateReceptionistCommandHandler( IReceptionistCommandRepository repository, IReceptionistReadRepository repoRead ) {
            this._repository = repository;
            this._repoRead = repoRead;
        }
        public async Task Handle( UpdateReceptionistCommand request, CancellationToken cancellationToken = default ) {
            var receptionist = await _repoRead.GetAsync( request.Id );
            if ( receptionist == null)
                throw new ReceptionistNotFoundException( request.Id.ToString() );
            var updatedReceptionist = (request, receptionist).Adapt<Receptionist>();
            await _repository.UpdateAsync( updatedReceptionist );
        }
    }
}
