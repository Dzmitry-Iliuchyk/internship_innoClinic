using Microsoft.EntityFrameworkCore;
using Services.Application.Abstractions.Repositories;
using Services.Domain;
using Shared.Abstractions.Repository;
using System.Linq.Expressions;

namespace Services.DataAccess.Repositories {
    public class ServiceRepository:BaseRepository<Service>, IServiceRepository {

        public ServiceRepository( ServiceContext context ): base(context) {
        }

        public async Task<bool> AnyAsync( Expression<Func<Service, bool>> filter ) {
            return await entities.AnyAsync(filter);
        }

        public async Task<Service?> GetAsync( Expression<Func<Service, bool>> filter ) {
            return await entities
                .AsNoTracking()
                .Include(s=>s.Specialization)
                .Include(s=>s.Category)
                .SingleOrDefaultAsync( filter );
        }
        public async Task<Service?> GetLightAsync( Expression<Func<Service, bool>> filter ) {
            return await entities
                .AsNoTracking()
                .SingleOrDefaultAsync(filter);
        }

    }
}
