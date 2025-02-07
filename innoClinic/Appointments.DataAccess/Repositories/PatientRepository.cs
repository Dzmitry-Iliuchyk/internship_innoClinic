using Appointments.Application.Interfaces.Repositories;
using Appointments.Domain;
using Microsoft.EntityFrameworkCore;
using Shared.Abstractions.Repository;

namespace Appointments.DataAccess.Repositories {
    public class PatientRepository: BaseRepository<Patient>, IPatientRepository {
        public PatientRepository( AppointmentsDbContext context ) : base( context ) {
        }

        public async Task<Patient?> GetAsync( Guid id ) {
            return await entities.AsNoTracking().FirstAsync(x=>x.Id == id);
        }
    }
}
