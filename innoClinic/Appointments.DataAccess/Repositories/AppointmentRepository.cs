using Appointments.Application.Interfaces.Repositories;
using Appointments.Domain;
using Microsoft.EntityFrameworkCore;
using Shared.Abstractions.Repository;

namespace Appointments.DataAccess.Repositories {
    public class AppointmentRepository: BaseRepository<Appointment>, IAppointmentRepository {
        public AppointmentRepository( AppointmentsDbContext context ) : base( context ) {
        }
        public override async Task<IList<Appointment>?> GetAllAsync() {
            return await entities.AsNoTracking()
                .Include( x => x.Office )
                .Include( x => x.Doctor )
                .Include( x => x.Service )
                .Include( x => x.Patient )
                .ToListAsync();
        }
        public async Task<Appointment?> GetAsync( Guid id ) {
            return await entities.AsNoTracking()
                .Include( x => x.Office )
                .Include( x => x.Doctor )
                .Include( x => x.Service )
                .Include( x => x.Patient )
                .SingleOrDefaultAsync( x => x.Id == id );
        }
    }
}
