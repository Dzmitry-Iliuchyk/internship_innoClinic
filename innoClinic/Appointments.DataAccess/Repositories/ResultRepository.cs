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
    public class DoctorRepository: BaseRepository<Doctor>, IDoctorRepository {
        public DoctorRepository( AppointmentsDbContext context ) : base( context ) {
        }

        public async Task<Doctor?> GetAsync( Guid id ) {
            return await entities.AsNoTracking().FirstAsync(x=>x.Id == id);
        }
    }
    public class ServiceRepository: BaseRepository<Service>, IServiceRepository {
        public ServiceRepository( AppointmentsDbContext context ) : base( context ) {
        }

        public async Task<Service?> GetAsync( int id ) {
            return await entities.AsNoTracking().FirstAsync(x=>x.Id == id);
        }
    }
    public class OfficeRepository: BaseRepository<Office>, IOfficeRepository {
        public OfficeRepository( AppointmentsDbContext context ) : base( context ) {
        }

        public async Task<Office?> GetAsync( string id ) {
            return await entities.AsNoTracking().FirstAsync(x=>x.Id == id);
        }
    }
    public class PatientRepository: BaseRepository<Patient>, IPatientRepository {
        public PatientRepository( AppointmentsDbContext context ) : base( context ) {
        }

        public async Task<Patient?> GetAsync( Guid id ) {
            return await entities.AsNoTracking().FirstAsync(x=>x.Id == id);
        }
    }
}
