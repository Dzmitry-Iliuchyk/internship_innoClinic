using Mapster;
using MediatR;
using Profiles.Application.Interfaces.Repositories;
using System.ComponentModel.DataAnnotations;

namespace Profiles.Application.Patients.Queries.GetFiltered {
    public record GetPatientsQuery(): IRequest<IList<PatientDto>>;

    public class GetPatientsQueryHandler: IRequestHandler<GetPatientsQuery, IList<PatientDto>> {
        private readonly IPatientReadRepository _repository;
        public GetPatientsQueryHandler( IPatientReadRepository repository ) {
            this._repository = repository;
        }
        public async Task<IList<PatientDto>> Handle( GetPatientsQuery request, CancellationToken cancellationToken = default ) {
            var patients = await _repository.GetAllAsync();
            return patients.Adapt<IList<PatientDto>>();
        }
    }
}
