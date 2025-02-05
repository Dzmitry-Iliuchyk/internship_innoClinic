using Appointments.Domain;

namespace Appointments.Application.Interfaces.Repositories {
    public interface IServiceRepository {
        Task<Service?> GetAsync( int id );
        Task CreateAsync( Service entity );
        Task UpdateAsync( Service entity );
        Task DeleteAsync( Service entity );
        Task<IList<Service>> GetAllAsync();
    }
}
