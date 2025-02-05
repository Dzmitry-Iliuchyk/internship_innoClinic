using Appointments.Domain;

namespace Appointments.Application.Interfaces.Repositories {
    public interface IDoctorRepository {
        Task<Doctor?> GetAsync( Guid id );
        Task CreateAsync( Doctor entity );
        Task UpdateAsync( Doctor entity );
        Task DeleteAsync( Doctor entity );
        Task<IList<Doctor>> GetAllAsync();
    }
}
