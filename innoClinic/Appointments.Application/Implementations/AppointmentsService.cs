using Appointments.Application.Dtos;
using Appointments.Application.Interfaces.Repositories;
using Appointments.Application.Interfaces.Services;
using Appointments.Domain;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appointments.Application.Implementations {
    public class AppointmentsService: IAppointmentService {
        private readonly IAppointmentRepository _repository;

        public AppointmentsService( IAppointmentRepository repository ) {
            this._repository = repository;
        }

        public async Task<Guid> CreateAsync( AppointmentCreateDto entity ) {
            var appointment = entity.Adapt<Appointment>();
            appointment.Id = Guid.NewGuid();

            await _repository.CreateAsync( appointment );

            return appointment.Id;
        }

        public async Task DeleteAsync( Guid id ) {
            var entity = await _repository.GetAsync(id);
            if (entity == null) {
                throw new NotImplementedException();    
            }
            await _repository.DeleteAsync( entity );
        }

        public async Task<IList<AppointmentDto>> GetAllAsync() {
            return (await _repository.GetAllAsync()).Adapt<IList<AppointmentDto>>();
        }

        public async Task<AppointmentDto> GetAsync( Guid id ) {
            return ( await _repository.GetAsync( id ) ).Adapt<AppointmentDto>( );
        }

        public async Task UpdateAsync( AppointmentUpdateDto entity ) {
            await _repository.UpdateAsync(entity.Adapt<Appointment>());
        }
    }
}
