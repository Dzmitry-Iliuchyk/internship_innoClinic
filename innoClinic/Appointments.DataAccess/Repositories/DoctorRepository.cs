using Appointments.Application.Interfaces.Repositories;
using Appointments.Domain;
using Microsoft.EntityFrameworkCore;
using Shared.Abstractions.Repository;

namespace Appointments.DataAccess.Repositories {
    public class DoctorRepository: BaseRepository<Doctor>, IDoctorRepository {
        public DoctorRepository( AppointmentsDbContext context ) : base( context ) {
        }

        public async Task<Doctor?> GetAsync( Guid id ) {
            return await entities.AsNoTracking().FirstAsync(x=>x.Id == id);
        }
    }
}
