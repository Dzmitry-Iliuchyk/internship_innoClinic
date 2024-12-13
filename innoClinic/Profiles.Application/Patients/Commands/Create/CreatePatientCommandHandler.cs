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

namespace Profiles.Application.Patients.Commands.Create {
    public record CreatePatientCommand( [Required] DateTime dateOfBirth,
                                        [Required] string firstName,
                                        [Required] string lastName,
                                        [Required] string email,
                                        [Required] string phoneNumber,
                                        [Required] Guid createdBy,
                                        [Required] string? photoUrl,
                                        [Required] string? middleName ): IRequest<Guid>;

    public class CreatePatientCommandHandler: IRequestHandler<CreatePatientCommand, Guid> {
        private readonly IPatientCommandRepository _repository;
        public CreatePatientCommandHandler( IPatientCommandRepository repository ) {
            this._repository = repository;
        }

        public async Task<Guid> Handle( CreatePatientCommand request, CancellationToken cancellationToken = default ) {
            var patient = request.Adapt<Patient>() ;
            await _repository.CreateAsync( patient );
            return patient.Id;
        }
    }
}
