using Appointments.Application.Dtos;
using Appointments.Application.Exceptions;
using Appointments.Application.Interfaces.Repositories;
using Appointments.Application.Interfaces.Services;
using Appointments.Domain;
using Mapster;
using MassTransit;
//using Shared.Events.Contracts;

namespace Appointments.Application.Implementations {
    public class AppointmentsService: IAppointmentService {
        private readonly IAppointmentRepository _repository;
        private readonly IPublishEndpoint _publisher;
        public AppointmentsService( IAppointmentRepository repository ) {
            this._repository = repository;
        }

        public async Task<Guid> CreateAsync( AppointmentCreateDto entity ) {
            var appointment = entity.Adapt<Appointment>();
            appointment.Id = Guid.NewGuid();

            await _repository.CreateAsync( appointment );
           // await _publisher.Publish( new AppointmentCreatedEvent() );
            return appointment.Id;
        }

        public async Task DeleteAsync( Guid id ) {
            var entity = await _repository.GetAsync(id);
            if (entity == null) {
                throw new AppointmentNotFoundException(id);    
            }
            await _repository.DeleteAsync( entity );
        }

        public async Task<IList<AppointmentDto>> GetAllAsync() {
            return (await _repository.GetAllAsync()).Adapt<IList<AppointmentDto>>();
        }

        public async Task<AppointmentDto> GetAsync( Guid id ) {
            var appointment = await _repository.GetAsync(id);
            if (appointment == null) {
                throw new AppointmentNotFoundException(id);
            }
            return appointment.Adapt<AppointmentDto>( );
        }
        public async Task<List<string>> GetEmailsAsync( Guid id ) {
            var appointment = await _repository.GetAsync(id);
            if (appointment == null) {
                throw new AppointmentNotFoundException(id);
            }
            return new List<string> {
                appointment.Patient.PatientEmail
            };
        }

        public async Task UpdateAsync( AppointmentUpdateDto entity ) {
            var appointment = await _repository.GetAsync( entity.Id );
            if (appointment == null) {
                throw new AppointmentNotFoundException( entity.Id );
            }
            await _repository.UpdateAsync(entity.Adapt<Appointment>());
        }
    }
}
