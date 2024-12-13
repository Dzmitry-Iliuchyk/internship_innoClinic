using Mapster;
using MediatR;
using Profiles.Application.Interfaces.Repositories;
using Profiles.Domain;
using System.ComponentModel.DataAnnotations;
using System.Security.AccessControl;

namespace Profiles.Application.Doctors.Commands.Create{
    public record CreateDoctorCommand( [Required] DateTime dateOfBirth,
                                       [Required] DateTime careerStartYear,
                                       [Required] string officeId,
                                       [Required] DoctorStatuses status,
                                       [Required] int specializationId,
                                       [Required] string firstName,
                                       [Required] string lastName,
                                       [Required] string email,
                                       [Required] string phoneNumber,
                                       Guid createdBy,
                                       string? photoUrl,
                                       string? middleName ): IRequest<Guid>;

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

