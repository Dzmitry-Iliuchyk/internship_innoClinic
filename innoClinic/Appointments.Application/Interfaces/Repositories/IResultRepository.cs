using Appointments.Domain;

namespace Appointments.Application.Interfaces.Repositories {
    public interface IResultRepository {
        Task<Result?> GetAsync( Guid id );
        Task CreateAsync( Result entity );
        Task UpdateAsync( Result entity );
        Task DeleteAsync( Result entity );
        Task<IList<Result>> GetAllAsync();
    }
}
