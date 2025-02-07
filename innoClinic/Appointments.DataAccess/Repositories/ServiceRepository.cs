using Appointments.Application.Interfaces.Repositories;
using Appointments.Domain;
using Microsoft.EntityFrameworkCore;
using Shared.Abstractions.Repository;

namespace Appointments.DataAccess.Repositories {
    public class ServiceRepository: BaseRepository<Service>, IServiceRepository {
        public ServiceRepository( AppointmentsDbContext context ) : base( context ) {
        }

        public async Task<Service?> GetAsync( Guid id ) {
            return await entities.AsNoTracking().FirstAsync( x => x.Id == id );
        }
    }
}
