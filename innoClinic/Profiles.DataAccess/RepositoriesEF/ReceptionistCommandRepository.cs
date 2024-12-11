using Microsoft.EntityFrameworkCore;
using Profiles.Application.Interfaces.Repositories;
using Profiles.Domain;
using Shared.Abstractions.Repository;

namespace Profiles.DataAccess.Repositories {
    public class ReceptionistCommandRepository: BaseRepository<Receptionist>, IReceptionistCommandRepository {
        public ReceptionistCommandRepository( ProfilesDbContext context ) : base( context ) {
        }
    }
}
