using Appointments.Application.Interfaces.Repositories;
using Appointments.Domain;
using Microsoft.EntityFrameworkCore;
using Shared.Abstractions.Repository;

namespace Appointments.DataAccess.Repositories {
    public class OfficeRepository: BaseRepository<Office>, IOfficeRepository {
        public OfficeRepository( AppointmentsDbContext context ) : base( context ) {
        }

        public async Task<Office?> GetAsync( string id ) {
            return await entities.AsNoTracking().FirstAsync(x=>x.Id == id);
        }
    }
}
