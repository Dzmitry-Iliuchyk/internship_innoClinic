using Microsoft.EntityFrameworkCore;
using Services.Application.Abstractions.Repositories;
using Services.Domain;
using Shared.Abstractions.Repository;
using System.Linq.Expressions;

namespace Services.DataAccess.Repositories {
    public class ServiceCategoryRepository:BaseRepository<ServiceCategory>, IServiceCategoryRepository {
        public ServiceCategoryRepository( ServiceContext context ) : base(context) {
        }

        public async Task<bool> AnyAsync( Expression<Func<ServiceCategory, bool>> filter ) {
            return await entities.AnyAsync( filter );
        }
        public override async Task<int> CreateAsync( ServiceCategory entity ) {
            await entities.AddAsync( entity );
            await dbContext.SaveChangesAsync();
            return entity.Id;
        }
        public async Task<ServiceCategory?> GetAsync( Expression<Func<ServiceCategory, bool>> filter ) {
            return await entities
                .AsNoTracking()
                .Include( s => s.Services )
                .SingleOrDefaultAsync(filter);
        }
        public async Task<ServiceCategory?> GetLightAsync( Expression<Func<ServiceCategory, bool>> filter ) {
            return await entities
                .AsNoTracking()
                .SingleOrDefaultAsync( filter );
        }
    }
}
