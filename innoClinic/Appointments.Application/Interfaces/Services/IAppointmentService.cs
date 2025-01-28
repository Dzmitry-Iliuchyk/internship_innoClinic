using Appointments.Application.Dtos;

namespace Appointments.Application.Interfaces.Services {
    public interface IAppointmentService {
        Task<AppointmentDto> GetAsync( Guid id );
        Task<Guid> CreateAsync( AppointmentCreateDto entity );
        Task UpdateAsync( AppointmentUpdateDto entity );
        Task DeleteAsync( Guid entity );
        Task<IList<AppointmentDto>> GetAllAsync();
        Task<List<string>> GetEmailsAsync( Guid id );
    }
}
