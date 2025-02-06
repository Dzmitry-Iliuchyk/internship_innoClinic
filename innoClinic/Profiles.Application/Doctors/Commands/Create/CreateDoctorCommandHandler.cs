using Mapster;
using MassTransit;
using MediatR;
using Profiles.Application.Common;
using Profiles.Application.Interfaces.Repositories;
using Profiles.Domain;
using Shared.Events.Contracts.ProfilesMessages;
using System.ComponentModel.DataAnnotations;
using System.Security.AccessControl;

namespace Profiles.Application.Doctors.Commands.Create {
    public record CreateDoctorCommand(
        [Required] DateTime DateOfBirth,
        [Required] DateTime CareerStartYear,
        [Required] string OfficeId,
        [Required] DoctorStatuses Status,
        [Required] int SpecializationId,
        [Required] string FirstName,
        [Required] string LastName,
        [Required] string Email,
        [Required] string PhoneNumber,
        Guid? CreatedBy,
        string? PhotoUrl,
        string? MiddleName ): CreatePersonCommandBase( FirstName, LastName, Email, PhoneNumber, CreatedBy, PhotoUrl, MiddleName );
    public class CreateDoctorCommandHandler: IRequestHandler<CreateDoctorCommand, Guid> {
        private readonly IDoctorCommandRepository _repository;
        private readonly IDoctorReadRepository _readRepository;
        private readonly IPublishEndpoint _publisher;
        public CreateDoctorCommandHandler( IDoctorCommandRepository repository, IPublishEndpoint publisher ) {
            this._repository = repository;
            this._publisher = publisher;
        }

        public async Task<Guid> Handle( CreateDoctorCommand request, CancellationToken cancellationToken = default ) {
            var doc = request.Adapt<Doctor>();
            await _repository.CreateAsync(doc);

            var doctor = await _readRepository.GetAsync(doc.Id);

            await _publisher.Publish<DoctorCreated>(new DoctorCreated {
                Id = doctor.Id,
                Email = doctor.Email,
                FirstName = doctor.FirstName,
                SecondName = doctor.LastName,
                Specialization = doctor.Specialization.Name
            } );
            return doc.Id;
        }
    }
}

