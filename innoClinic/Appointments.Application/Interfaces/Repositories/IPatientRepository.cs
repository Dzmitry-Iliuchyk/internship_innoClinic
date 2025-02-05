using Appointments.Domain;

namespace Appointments.Application.Interfaces.Repositories {
    public interface IPatientRepository {
        Task<Patient?> GetAsync( Guid id );
        Task CreateAsync( Patient entity );
        Task UpdateAsync( Patient entity );
        Task DeleteAsync( Patient entity );
        Task<IList<Patient>> GetAllAsync();
    }
}
