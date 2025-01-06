using Appointments.Domain;

namespace Appointments.Application.Interfaces.Repositories {
    public interface IAppointmentRepository {
        Task<Appointment?> GetAsync( Guid id );
        Task CreateAsync( Appointment entity );
        Task UpdateAsync( Appointment entity );
        Task DeleteAsync( Appointment entity );
        Task<IList<Appointment>> GetAllAsync();
    }
}
