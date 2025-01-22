using Offices.Domain.Models;

namespace Offices.Application.Interfaces.Repositories {
    public interface IOfficeRepository {
        Task<Office> GetAsync(string id);
        Task<List<Office>> GetAllAsync();
        Task UpdateAsync(Office entity);
        Task DeleteAsync(string id);
        Task CreateAsync(Office entity);
        Task<bool> AnyAsync( string id );
        Task<bool> AnyByNumberAsync( string phone );
        Task SetPathToOffice( string id, string path );
    }
}
