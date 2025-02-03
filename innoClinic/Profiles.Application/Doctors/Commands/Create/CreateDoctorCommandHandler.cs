using Mapster;
using MediatR;
using Profiles.Application.Common;
using Profiles.Application.Interfaces.Repositories;
using Profiles.Domain;
using System.ComponentModel.DataAnnotations;
using System.Security.AccessControl;

namespace Profiles.Application.Doctors.Commands.Create{
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
        [Required] Guid CreatedBy,
        string? PhotoUrl,
        string? MiddleName ): CreatePersonCommandBase( FirstName, LastName, Email, PhoneNumber, CreatedBy, PhotoUrl, MiddleName );
    public class CreateDoctorCommandHandler: IRequestHandler<CreateDoctorCommand, Guid> {
        private readonly IDoctorCommandRepository _repository;
        public CreateDoctorCommandHandler( IDoctorCommandRepository repository ) {
            this._repository = repository;
        }

        public async Task<Guid> Handle( CreateDoctorCommand request, CancellationToken cancellationToken = default ) {
            var doc = request.Adapt<Doctor>();
            await _repository.CreateAsync(doc);
            return doc.Id;
        }
    }
}

