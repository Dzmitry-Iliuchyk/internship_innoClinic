using Mapster;
using MediatR;
using Profiles.Application.Common.Exceptions;
using Profiles.Application.Interfaces.Repositories;
using Profiles.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profiles.Application.Patients.Commands.Update {
    public record UpdatePatientCommand( [Required] Guid id,
                                        [Required] DateTime dateOfBirth,
                                        [Required] string firstName,
                                        [Required] string lastName,
                                        [Required] string email,
                                        [Required] string phoneNumber,
                                        [Required] Guid updatedBy,
                                        [Required] string? photoUrl,
                                        [Required] string? middleName ): IRequest;

    public class UpdatePatientCommandHandler: IRequestHandler<UpdatePatientCommand> {
        private readonly IPatientReadRepository _repoRead;
        private readonly IPatientCommandRepository _repository;
        public UpdatePatientCommandHandler( IPatientCommandRepository repository, IPatientReadRepository repoRead ) {
            this._repository = repository;
            this._repoRead = repoRead;
        }

        public async Task Handle( UpdatePatientCommand request, CancellationToken cancellationToken = default ) {
            var patient = await _repoRead.GetAsync( request.id);
            if (patient == null) {
                throw new PatientNotFoundException( request.id.ToString() );
            }
            var updatedPatient = (request, patient).Adapt<Patient>();
            await _repository.UpdateAsync( updatedPatient );
        }
    }
}
