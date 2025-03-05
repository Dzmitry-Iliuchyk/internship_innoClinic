using Mapster;
using MediatR;
using Profiles.Application.Common.Security;
using Profiles.Application.Interfaces.Repositories;
using Shared.Abstractions.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profiles.Application.Doctors.Queries.GetPage {
    public record GetPagedDoctorsQuery(int skip, int take): IRequest<PagedResult<DoctorDto>>;

    public class GetDoctorsQueryHandler: IRequestHandler<GetPagedDoctorsQuery, PagedResult<DoctorDto>> {
        private readonly IDoctorReadRepository _repository;
        public GetDoctorsQueryHandler( IDoctorReadRepository repository ) {
            this._repository = repository;
        }

        public async Task<PagedResult<DoctorDto>> Handle( GetPagedDoctorsQuery request, CancellationToken cancellationToken = default ) {
            var doc = await _repository.GetPageAsync(request.skip, request.take);
            var paged = new PagedResult<DoctorDto>(doc.TotalCount,doc.Items.Adapt<IList<DoctorDto>>());
            return paged;
        }
    }
}
