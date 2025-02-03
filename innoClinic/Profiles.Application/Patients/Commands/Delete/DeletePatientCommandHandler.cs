using MediatR;
using Profiles.Application.Common.Exceptions;
using Profiles.Application.Interfaces.Repositories;
using System.ComponentModel.DataAnnotations;

namespace Profiles.Application.Patients.Commands.Delete {
    public record DeletePatientCommand( [Required] Guid patientId ): IRequest<Unit>;

    public class DeletePatientCommandHandler: IRequestHandler<DeletePatientCommand, Unit> {
        private readonly IPatientCommandRepository _repository;
        private readonly IPatientReadRepository _repoRead;
        public DeletePatientCommandHandler( IPatientCommandRepository repository, IPatientReadRepository repoRead ) {
            this._repository = repository;
            this._repoRead = repoRead;
        }

        public async Task<Unit> Handle( DeletePatientCommand request, CancellationToken cancellationToken = default ) {
            var doc = await _repoRead.GetAsync( request.patientId );
            if (doc != null) {
                await _repository.DeleteAsync( doc );
                return Unit.Value;
            } else
                throw new PatientNotFoundException( request.patientId.ToString() );
        }

    }
}
