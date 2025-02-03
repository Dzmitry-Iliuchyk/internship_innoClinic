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

namespace Profiles.Application.Doctors.Commands.Delete {
    public record DeleteDoctorCommand( [Required] Guid doctorId ): IRequest<Unit>;

    public class DeleteDoctorCommandHandler: IRequestHandler<DeleteDoctorCommand,Unit> {
        private readonly IDoctorCommandRepository _repository;
        private readonly IDoctorReadRepository _readRepo;
        public DeleteDoctorCommandHandler( IDoctorCommandRepository repository, IDoctorReadRepository readRepo ) {
            this._repository = repository;
            this._readRepo = readRepo;
        }

        public async Task<Unit> Handle( DeleteDoctorCommand request, CancellationToken cancellationToken = default ) {
            var doc = await _readRepo.GetAsync(request.doctorId);
            if (doc != null) {
                await _repository.DeleteAsync( doc );
                return Unit.Value;
            } else
                throw new DoctorNotFoundException(request.doctorId.ToString());
        }
    }
}
