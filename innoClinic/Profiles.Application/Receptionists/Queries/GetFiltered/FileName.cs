using Mapster;
using MediatR;
using Profiles.Application.Interfaces.Repositories;

namespace Profiles.Application.Receptionists.Queries.GetFiltered {
    public record GetReceptionistsQuery( ): IRequest<IList<ReceptionistDto>>;

    public class GetReceptionistsQueryHandler: IRequestHandler<GetReceptionistsQuery, IList<ReceptionistDto>> {
        private readonly IReceptionistReadRepository _repository;
        public GetReceptionistsQueryHandler( IReceptionistReadRepository repository ) {
            this._repository = repository;
        }
        public async Task<IList<ReceptionistDto>> Handle( GetReceptionistsQuery request, CancellationToken cancellationToken = default ) {
            var receptionists = await _repository.GetAllAsync( );
            return receptionists.Adapt<IList<ReceptionistDto>>();
        }
    }
}
