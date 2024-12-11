using Mapster;
using MediatR;
using Profiles.Application.Interfaces.Repositories;
using Profiles.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profiles.Application.Receptionists.Commands.Update {
    public record UpdateReceptionistCommand( [Required] Guid id,
                                             [Required] string officeId,
                                             [Required] string firstName,
                                             [Required] string lastName,
                                             [Required] string email,
                                             [Required] string phoneNumber,
                                             [Required] Guid updatedBy,
                                             [Required] string? photoUrl,
                                             [Required] string? middleName ): IRequest;

    public class UpdateReceptionistCommandHandler: IRequestHandler<UpdateReceptionistCommand> {
        private readonly IReceptionistCommandRepository _repository;
        private readonly IReceptionistReadRepository _repoRead;
        public UpdateReceptionistCommandHandler( IReceptionistCommandRepository repository, IReceptionistReadRepository repoRead ) {
            this._repository = repository;
            this._repoRead = repoRead;
        }
        public async Task Handle( UpdateReceptionistCommand request, CancellationToken cancellationToken = default ) {
            var receptionist = await _repoRead.GetAsync( request.id );
            var updatedReceptionist = (request, receptionist).Adapt<Receptionist>();
            await _repository.UpdateAsync( updatedReceptionist );
        }
    }
}
