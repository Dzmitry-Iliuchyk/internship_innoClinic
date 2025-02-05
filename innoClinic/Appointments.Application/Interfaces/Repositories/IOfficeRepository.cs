using Appointments.Domain;

namespace Appointments.Application.Interfaces.Repositories {
    public interface IOfficeRepository {
        Task<Office?> GetAsync( string id );
        Task CreateAsync( Office entity );
        Task UpdateAsync( Office entity );
        Task DeleteAsync( Office entity );
        Task<IList<Office>> GetAllAsync();
    }
}
