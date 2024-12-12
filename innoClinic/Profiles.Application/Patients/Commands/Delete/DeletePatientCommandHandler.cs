using MediatR;
using Profiles.Application.Common.Exceptions;
using Profiles.Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profiles.Application.Patients.Commands.Delete {
    public record DeletePatientCommand( [Required] Guid patientId ): IRequest;

    public class DeletePatientCommandHandler: IRequestHandler<DeletePatientCommand> {
        private readonly IPatientCommandRepository _repository;
        private readonly IPatientReadRepository _repoRead;
        public DeletePatientCommandHandler( IPatientCommandRepository repository, IPatientReadRepository repoRead ) {
            this._repository = repository;
            this._repoRead = repoRead;
        }

        public async Task Handle( DeletePatientCommand request, CancellationToken cancellationToken = default ) {
            var doc = await _repoRead.GetAsync( request.patientId );
            if (doc != null)
                await _repository.DeleteAsync( doc );
            else
                throw new PatientNotFoundException( request.patientId.ToString() );
        }
    }
}
