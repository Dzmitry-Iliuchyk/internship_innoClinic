using MassTransit;
using MediatR;
using Profiles.Application.Common.Exceptions;
using Profiles.Application.Interfaces.Repositories;
using Profiles.Domain;
using Shared.Events.Contracts;
using System.ComponentModel.DataAnnotations;

namespace Profiles.Application.Patients.Commands.Delete {
    public record DeletePatientCommand( [Required] Guid patientId ): IRequest<Unit>;

    public class DeletePatientCommandHandler: IRequestHandler<DeletePatientCommand, Unit> {
        private readonly IPatientCommandRepository _repository;
        private readonly IPatientReadRepository _repoRead;
        private readonly IPublishEndpoint _publisher;
        public DeletePatientCommandHandler( IPatientCommandRepository repository, IPatientReadRepository repoRead, IPublishEndpoint publisher ) {
            this._repository = repository;
            this._repoRead = repoRead;
            this._publisher = publisher;
        }

        public async Task<Unit> Handle( DeletePatientCommand request, CancellationToken cancellationToken = default ) {
            var patient = await _repoRead.GetAsync( request.patientId );
            if (patient != null) {
                await _repository.DeleteAsync( patient );
                
            } else
                throw new PatientNotFoundException( request.patientId.ToString() );

            await _publisher.Publish<PatientDeleted>( new PatientDeleted {
                Id = patient.Id,
            } );
            return Unit.Value;
        }

    }
}
