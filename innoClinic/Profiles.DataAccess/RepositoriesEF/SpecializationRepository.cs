using Microsoft.EntityFrameworkCore;
using Profiles.Application.Interfaces.Repositories;
using Profiles.Domain;
using Shared.Abstractions.Repository;
using System.Linq.Expressions;

namespace Profiles.DataAccess.Repositories {
    public class SpecializationRepository: BaseRepository<Specialization>, ISpecializationRepository {
        public SpecializationRepository( ProfilesDbContext context ) : base( context ) {
        }

        public async Task<Specialization?> GetAsync( Expression<Func<Specialization, bool>> filter ) {
            return await entities.AsNoTracking().Where( filter ).SingleOrDefaultAsync();
        }
        
    }
}
