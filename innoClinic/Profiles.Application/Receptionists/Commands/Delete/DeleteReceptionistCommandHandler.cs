using MediatR;
using Profiles.Application.Common.Exceptions;
using Profiles.Application.Interfaces.Repositories;
using System.ComponentModel.DataAnnotations;

namespace Profiles.Application.Receptionists.Commands.Delete {
    public record DeleteReceptionistCommand( [Required] Guid receptionistId ): IRequest<Unit>;

    public class DeleteReceptionistCommandHandler: IRequestHandler<DeleteReceptionistCommand, Unit> {
        private readonly IReceptionistCommandRepository _repository;
        private readonly IReceptionistReadRepository _repoRead;
        public DeleteReceptionistCommandHandler( IReceptionistCommandRepository repository, IReceptionistReadRepository repoRead ) {
            this._repository = repository;
            this._repoRead = repoRead;
        }

        public async Task<Unit> Handle( DeleteReceptionistCommand request, CancellationToken cancellationToken = default ) {
            var doc = await _repoRead.GetAsync( request.receptionistId );
            if (doc != null) {
                await _repository.DeleteAsync( doc );
                return Unit.Value;
            } else
                throw new ReceptionistNotFoundException( request.receptionistId.ToString() );

        }
    }
}
