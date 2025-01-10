using Microsoft.EntityFrameworkCore;
using Services.Application.Abstractions.Repositories;
using Services.Domain;
using Shared.Abstractions.Repository;
using System.Linq.Expressions;

namespace Services.DataAccess.Repositories {
    public class SpecializationRepository: BaseRepository<Specialization>, ISpecializationRepository {
        public SpecializationRepository( ServiceContext context ) : base(context) {
        }

        public async Task<bool> AnyAsync( Expression<Func<Specialization, bool>> filter ) {
            return await entities.AnyAsync( filter );
        }
        public override async Task<int> CreateAsync( Specialization entity ) {
            await entities.AddAsync( entity );
            await dbContext.SaveChangesAsync();
            return entity.Id;
        }
        public async Task<Specialization?> GetAsync( Expression<Func<Specialization, bool>> filter ) {
            return await entities
                .AsNoTracking()
                .Include( s => s.Services )
                .SingleOrDefaultAsync( filter );
        }
        public async Task<Specialization?> GetLightAsync( Expression<Func<Specialization, bool>> filter ) {
            return await entities
                .AsNoTracking()
                .SingleOrDefaultAsync( filter );
        }
    }
}
