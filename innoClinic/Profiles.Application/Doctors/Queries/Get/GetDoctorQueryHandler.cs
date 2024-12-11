using Mapster;
using MediatR;
using Profiles.Application.Interfaces.Repositories;
using System.ComponentModel.DataAnnotations;

namespace Profiles.Application.Doctors.Queries.Get {
    public record GetDoctorQuery( [Required] Guid id): IRequest<DoctorDto>;

    public class GetDoctorQueryHandler: IRequestHandler<GetDoctorQuery, DoctorDto> {
        private readonly IDoctorReadRepository _read;
        public GetDoctorQueryHandler( IDoctorReadRepository repository ) {
            this._read = repository;
        }

        public async Task<DoctorDto> Handle( GetDoctorQuery request, CancellationToken cancellationToken = default ) {
            var doc = await _read.GetAsync( request.id );
            return doc.Adapt<DoctorDto>();
        }
    }
}
