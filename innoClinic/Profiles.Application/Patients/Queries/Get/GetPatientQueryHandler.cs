using Mapster;
using MediatR;
using Profiles.Application.Doctors.Queries.Get;
using Profiles.Application.Interfaces.Repositories;
using System.ComponentModel.DataAnnotations;

namespace Profiles.Application.Patients.Queries.Get {
    public record GetPatientQuery( [Required] Guid id ): IRequest<PatientDto>;

    public class GetPatientQueryHandler: IRequestHandler<GetPatientQuery, PatientDto> {
        private readonly IPatientReadRepository _repository;
        public GetPatientQueryHandler( IPatientReadRepository repository ) {
            this._repository = repository;
        }
        public async Task<PatientDto> Handle( GetPatientQuery request, CancellationToken cancellationToken = default ) {
            var patient = await _repository.GetAsync( request.id );
            return patient.Adapt<PatientDto>();
        }
    }
}
