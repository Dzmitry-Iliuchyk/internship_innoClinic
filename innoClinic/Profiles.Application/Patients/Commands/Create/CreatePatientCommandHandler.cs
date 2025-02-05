using Mapster;
using MassTransit;
using MediatR;
using Profiles.Application.Common;
using Profiles.Application.Interfaces.Repositories;
using Profiles.Domain;
using Shared.Events.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profiles.Application.Patients.Commands.Create {
    public record CreatePatientCommand(
    [Required] DateTime DateOfBirth,
    [Required] string FirstName,
    [Required] string LastName,
    [Required] string Email,
    [Required] string PhoneNumber,
    [Required] Guid CreatedBy,
    string? PhotoUrl,
    string? MiddleName )
        : CreatePersonCommandBase( FirstName, LastName, Email, PhoneNumber, CreatedBy, PhotoUrl, MiddleName );

    public class CreatePatientCommandHandler: IRequestHandler<CreatePatientCommand, Guid> {
        private readonly IPatientCommandRepository _repository;
        private readonly IPublishEndpoint _publisher;
        public CreatePatientCommandHandler( IPatientCommandRepository repository, IPublishEndpoint publisher ) {
            this._repository = repository;
            this._publisher = publisher;
        }

        public async Task<Guid> Handle( CreatePatientCommand request, CancellationToken cancellationToken = default ) {
            var patient = request.Adapt<Patient>() ;
            await _repository.CreateAsync( patient );


            await _publisher.Publish<PatientCreated>( new PatientCreated {
                Id = patient.Id,
                Email = patient.Email,
                FirstName = patient.FirstName,
                SecondName = patient.LastName,
            } );
            return patient.Id;
        }
    }
}
