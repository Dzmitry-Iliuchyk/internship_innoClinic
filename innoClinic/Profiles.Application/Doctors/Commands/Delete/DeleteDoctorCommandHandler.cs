using MassTransit;
using MediatR;
using Profiles.Application.Common.Exceptions;
using Profiles.Application.Interfaces.Repositories;
using Profiles.Domain;
using Shared.Events.Contracts;
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
        private readonly IPublishEndpoint _publisher;
        public DeleteDoctorCommandHandler( IDoctorCommandRepository repository, IDoctorReadRepository readRepo, IPublishEndpoint publisher ) {
            this._repository = repository;
            this._readRepo = readRepo;
            this._publisher = publisher;
        }

        public async Task<Unit> Handle( DeleteDoctorCommand request, CancellationToken cancellationToken = default ) {
            var doc = await _readRepo.GetAsync(request.doctorId);
            if (doc != null) {
                await _repository.DeleteAsync( doc );
            } else
                throw new DoctorNotFoundException(request.doctorId.ToString());


            await _publisher.Publish<DoctorDeleted>( new DoctorDeleted {
                Id = doc.Id,
            } );
            return Unit.Value;
        }
    }
}
