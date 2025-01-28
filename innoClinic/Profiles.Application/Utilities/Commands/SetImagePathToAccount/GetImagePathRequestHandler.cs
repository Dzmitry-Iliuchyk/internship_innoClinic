using MediatR;
using Profiles.Application.Interfaces.Repositories;
using System.ComponentModel.DataAnnotations;

namespace Profiles.Application.Utilities.Commands.SetImagePathToAccount {
    public record GetImagePathRequest( [Required] Guid id ): IRequest<string>;
    public class GetImagePathRequestHandler: IRequestHandler<GetImagePathRequest, string> {
        private readonly IAccountReadRepository _repository;
        public GetImagePathRequestHandler( IAccountReadRepository repository ) {
            this._repository = repository;
        }

        public async Task<string> Handle( GetImagePathRequest request, CancellationToken cancellationToken ) {
            return await _repository.GetPathToImage( request.id );
        }
    }
}