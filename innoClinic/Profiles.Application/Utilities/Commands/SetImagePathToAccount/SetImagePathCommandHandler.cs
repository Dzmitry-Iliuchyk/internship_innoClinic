using Mapster;
using MediatR;
using Profiles.Application.Interfaces.Repositories;
using Profiles.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profiles.Application.Utilities.Commands.SetImagePathToAccount {
    public record SetImagePathCommand( [Required] Guid id,
                                       [Required] string path
                                         ): IRequest;

    public class SetImagePathCommandHandler: IRequestHandler<SetImagePathCommand> {
        private readonly IAccountRepository _repository;
        public SetImagePathCommandHandler( IAccountRepository repository ) {
            this._repository = repository;
        }
        public async Task Handle( SetImagePathCommand request, CancellationToken cancellationToken ) {
            await _repository.SetPathToImage(request.id, request.path);
        }
    }
    
}
