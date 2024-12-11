using MediatR;
using Profiles.Application.Interfaces.Repositories;
using Profiles.Application.Patients.Queries.Get;
using Profiles.Application.Patients.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mapster;
using Profiles.Domain;
using System.ComponentModel.DataAnnotations;

namespace Profiles.Application.Receptionists.Queries.Get {
    public record GetReceptionistQuery( [Required] Guid receptionistId):IRequest<ReceptionistDto>;

    public class GetReceptionistQueryHandler: IRequestHandler<GetReceptionistQuery, ReceptionistDto> {
        private readonly IReceptionistReadRepository _repoRead;
        public GetReceptionistQueryHandler( IReceptionistReadRepository repoREad ) {
            this._repoRead = repoREad;
        }
        public async Task<ReceptionistDto> Handle( GetReceptionistQuery request, CancellationToken cancellationToken = default ) {
            var receptionist = await _repoRead.GetAsync( request.receptionistId );
            return receptionist.Adapt<ReceptionistDto>();
        }
    }
}
