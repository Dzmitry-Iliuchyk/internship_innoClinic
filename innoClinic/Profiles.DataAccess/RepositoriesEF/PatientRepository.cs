using Microsoft.EntityFrameworkCore;
using Profiles.Application.Interfaces.Repositories;
using Profiles.Domain;
using Shared.Abstractions.Entities;
using Shared.Abstractions.Repository;

namespace Profiles.DataAccess.Repositories {
    public class PatientCommandRepository: BaseRepository<Patient>, IPatientCommandRepository {
        public PatientCommandRepository( ProfilesDbContext context ) : base( context ) {
        }
    }
}
