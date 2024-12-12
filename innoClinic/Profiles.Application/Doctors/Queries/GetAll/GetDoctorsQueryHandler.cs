using Mapster;
using MediatR;
using Profiles.Application.Common.Security;
using Profiles.Application.Interfaces.Repositories;
using System.ComponentModel.DataAnnotations;

namespace Profiles.Application.Doctors.Queries.GetFiltered {
    [Authorize( Roles = "Doctor" )]
    public record GetDoctorsQuery( ): IRequest<IList<DoctorDto>>;

    public class GetDoctorsQueryHandler: IRequestHandler<GetDoctorsQuery, IList<DoctorDto>> {
        private readonly IDoctorReadRepository _repository;
        public GetDoctorsQueryHandler( IDoctorReadRepository repository ) {
            this._repository = repository;
        }

        public async Task<IList<DoctorDto>> Handle( GetDoctorsQuery request, CancellationToken cancellationToken = default ) {
            var doc = await _repository.GetAllAsync( );
            return doc.Adapt<IList<DoctorDto>>();
        }
    }
}
