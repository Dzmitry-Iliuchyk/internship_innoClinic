using Appointments.Application.Interfaces.Repositories;
using Appointments.Domain;
using Microsoft.EntityFrameworkCore;
using Shared.Abstractions.Repository;

namespace Appointments.DataAccess.Repositories {
    public class AppointmentRepository: BaseRepository<Appointment>, IAppointmentRepository {
        public AppointmentRepository( AppointmentsDbContext context ) : base( context ) {
        }

        public async Task<Appointment?> GetAsync( Guid id ) {
            return await entities.AsNoTracking().SingleOrDefaultAsync( x => x.Id == id );
        }
    }
}
