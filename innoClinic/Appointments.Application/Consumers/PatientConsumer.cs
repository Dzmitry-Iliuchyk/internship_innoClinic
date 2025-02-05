using Appointments.Application.Interfaces.Repositories;
using MassTransit;
using Shared.Events.Contracts;

namespace Appointments.Application.Consumers {
    public class PatientCreatedConsumer: IConsumer<PatientCreated> {
        private readonly IPatientRepository _patients;
        public PatientCreatedConsumer( IPatientRepository patients ) {
            this._patients = patients;

        }

        public async Task Consume( ConsumeContext<PatientCreated> context ) {

            await _patients.CreateAsync( new Domain.Patient {
                Id = context.Message.Id,
                PatientEmail = context.Message.Email,
                PatientFirstName = context.Message.FirstName,
                PatientSecondName = context.Message.SecondName,
            } );

        }
    }
    public class PatientUpdatedConsumer: IConsumer<PatientUpdated> {
        private readonly IPatientRepository _patients;
        public PatientUpdatedConsumer( IPatientRepository patients ) {
            this._patients = patients;

        }

        public async Task Consume( ConsumeContext<PatientUpdated> context ) {

            await _patients.UpdateAsync( new Domain.Patient {
                Id = context.Message.Id,
                PatientEmail = context.Message.Email,
                PatientFirstName = context.Message.FirstName,
                PatientSecondName = context.Message.SecondName,
            } );

        }
    }
    public class PatientDeletedConsumer: IConsumer<PatientDeleted> {
        private readonly IPatientRepository _patients;
        public PatientDeletedConsumer( IPatientRepository patients ) {
            this._patients = patients;
        }

        public async Task Consume( ConsumeContext<PatientDeleted> context ) {

            var pat = await _patients.GetAsync(context.Message.Id);
            await _patients.DeleteAsync( pat );

        }
    }
}
