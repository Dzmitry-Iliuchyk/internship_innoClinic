using Appointments.Application.Interfaces.Repositories;
using Appointments.Domain;
using Microsoft.EntityFrameworkCore;
using Shared.Abstractions.Repository;

namespace Appointments.DataAccess.Repositories {
    public class ResultRepository: BaseRepository<Result>, IResultRepository {
        public ResultRepository( AppointmentsDbContext context ) : base( context ) {
        }


        public async Task<Result?> GetAsync( Guid id ) {
            return await entities.AsNoTracking().OrderByDescending(x=>x.CreatedDate).FirstAsync(x=>x.Id== id);
        }
    }
}
