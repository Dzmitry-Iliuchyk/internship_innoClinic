using Appointments.Application.Interfaces.Repositories;
using MassTransit;
using Shared.Events.Contracts.ProfilesMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appointments.Application.Consumers {
    public class DoctorCreatedConsumer: IConsumer<DoctorCreated> {
        private readonly IDoctorRepository _doctors;
        public DoctorCreatedConsumer( IDoctorRepository patients ) {
            this._doctors = patients;

        }
        public async Task Consume( ConsumeContext<DoctorCreated> context ) {

            await _doctors.CreateAsync( new Domain.Doctor {
                Id = context.Message.Id,
                DoctorFirstName = context.Message.Email,
                DoctorSecondName = context.Message.FirstName,
                DoctorSpecialization = context.Message.Specialization,
            } );

        }
    }
    public class DoctorUpdatedConsumer: IConsumer<DoctorUpdated> {
        private readonly IDoctorRepository _doctors;
        public DoctorUpdatedConsumer( IDoctorRepository patients ) {
            this._doctors = patients;

        }

        public async Task Consume( ConsumeContext<DoctorUpdated> context ) {

            await _doctors.UpdateAsync( new Domain.Doctor {
                Id = context.Message.Id,
                DoctorFirstName = context.Message.FirstName,
                DoctorSecondName = context.Message.SecondName,
                DoctorSpecialization = context.Message.Specialization,
            } );

        }
    }
    public class DoctorDeletedConsumer: IConsumer<DoctorDeleted> {
        private readonly IDoctorRepository _doctors;
        public DoctorDeletedConsumer( IDoctorRepository patients ) {
            this._doctors = patients;
        }

        public async Task Consume( ConsumeContext<DoctorDeleted> context ) {

            var pat = await _doctors.GetAsync( context.Message.Id );
            await _doctors.DeleteAsync( pat );

        }
    }
}
