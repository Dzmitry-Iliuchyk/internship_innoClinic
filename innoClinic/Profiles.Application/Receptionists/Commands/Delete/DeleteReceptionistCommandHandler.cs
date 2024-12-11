using MediatR;
using Profiles.Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profiles.Application.Receptionists.Commands.Delete {
    public record DeleteReceptionistCommand( [Required] Guid receptionistId ): IRequest;

    public class DeleteReceptionistCommandHandler: IRequestHandler<DeleteReceptionistCommand> {
        private readonly IReceptionistCommandRepository _repository;
        private readonly IReceptionistReadRepository _repoRead;
        public DeleteReceptionistCommandHandler( IReceptionistCommandRepository repository, IReceptionistReadRepository repoRead ) {
            this._repository = repository;
            this._repoRead = repoRead;
        }

        public async Task Handle( DeleteReceptionistCommand request, CancellationToken cancellationToken = default ) {
            var doc = await _repoRead.GetAsync( request.receptionistId );
            if (doc != null)
                await _repository.DeleteAsync( doc );
        }
    }
}
