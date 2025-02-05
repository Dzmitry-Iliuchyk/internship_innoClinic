using MassTransit;
using MediatR;
using Profiles.Application.Common.Exceptions;
using Profiles.Application.Interfaces.Repositories;
using Shared.Events.Contracts;
using System.ComponentModel.DataAnnotations;

namespace Profiles.Application.Receptionists.Commands.Delete {
    public record DeleteReceptionistCommand( [Required] Guid receptionistId ): IRequest<Unit>;

    public class DeleteReceptionistCommandHandler: IRequestHandler<DeleteReceptionistCommand, Unit> {
        private readonly IReceptionistCommandRepository _repository;
        private readonly IReceptionistReadRepository _repoRead;
        private readonly IPublishEndpoint _publisher;
        public DeleteReceptionistCommandHandler( IReceptionistCommandRepository repository, IReceptionistReadRepository repoRead ) {
            this._repository = repository;
            this._repoRead = repoRead;
        }

        public async Task<Unit> Handle( DeleteReceptionistCommand request, CancellationToken cancellationToken = default ) {
            var receptionist = await _repoRead.GetAsync( request.receptionistId );
            if (receptionist != null) {
                await _repository.DeleteAsync( receptionist );
                
            } else
                throw new ReceptionistNotFoundException( request.receptionistId.ToString() );

            await _publisher.Publish<ReceptionistDeleted>( new ReceptionistDeleted {
                Id = receptionist.Id,
            } );
            return Unit.Value;
        }
    }
}
