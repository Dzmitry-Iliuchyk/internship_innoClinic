using Mapster;
using MediatR;
using Profiles.Application.Common.Exceptions;
using Profiles.Application.Interfaces.Repositories;
using Profiles.Domain;
using System.ComponentModel.DataAnnotations;

namespace Profiles.Application.Doctors.Commands.Update {
    public record UpdateDoctorCommand( [Required] Guid doctorId,
                                       [Required] DateTime dateOfBirth,
                                       [Required] DateTime careerStartYear,
                                       [Required] string officeId,
                                       [Required] DoctorStatuses status,
                                       [Required] int specializationId,
                                       [Required] string firstName,
                                       [Required] string lastName,
                                       [Required] string email,
                                       [Required] string phoneNumber,
                                       [Required] Guid updatedBy,
                                       string? photoUrl,
                                       string? middleName ): IRequest;

    public class UpdateDoctorCommandHandler: IRequestHandler<UpdateDoctorCommand> {
        private readonly IDoctorCommandRepository _repository;
        private readonly IDoctorReadRepository _readRepo;
        public UpdateDoctorCommandHandler( IDoctorCommandRepository repository, IDoctorReadRepository readRepo ) {
            this._repository = repository;
            this._readRepo = readRepo;
        }

        public async Task Handle( UpdateDoctorCommand request, CancellationToken cancellationToken = default ) {
            var doc = await _readRepo.GetAsync( request.doctorId );
            if (doc == null) {
                throw new DoctorNotFoundException( request.doctorId.ToString() );
            }
            await _repository.UpdateAsync( (request, doc).Adapt<Doctor>() );
        }
    }

}
