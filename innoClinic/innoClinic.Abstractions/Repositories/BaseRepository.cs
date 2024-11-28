using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Shared.Abstractions.Repository {
    public abstract class BaseRepository<TEntity>: IRepository<TEntity> where TEntity : class {

        protected readonly DbContext dbContext;
        protected readonly DbSet<TEntity> entities;
        public BaseRepository( DbContext context ) {
            dbContext = context;
            entities = context.Set<TEntity>();
        }
        public async Task CreateAsync( TEntity entity ) {
            await entities.AddAsync( entity );
            await dbContext.SaveChangesAsync();
        }
        public async Task DeleteAsync( TEntity entity ) {
            entities.Remove( entity );
            await dbContext.SaveChangesAsync();
        }
        public async Task UpdateAsync( TEntity updatedEntity ) {
            entities.Update( updatedEntity );
            await dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<TEntity>?> GetAllAsync() {
            return await entities
             .AsNoTracking()
             .ToArrayAsync();
        }
    }
}
