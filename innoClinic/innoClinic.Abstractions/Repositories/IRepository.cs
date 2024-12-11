namespace Shared.Abstractions.Repository {
    public interface IRepository<TEntity> {
        Task UpdateAsync( TEntity updatedEntity );
        Task CreateAsync( TEntity entity );
        Task DeleteAsync( TEntity entity );
        Task<IList<TEntity>?> GetAllAsync();
    }
}
