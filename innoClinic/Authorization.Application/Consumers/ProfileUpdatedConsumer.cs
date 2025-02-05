using Authorization.Application.Abstractions.Services;
using MassTransit;
using Shared.Events.Contracts;

namespace Authorization.Application.Consumers {
    public class PatientUpdatedConsumer: IConsumer<PatientUpdated> {
        private readonly IAuthService _auth;

        public PatientUpdatedConsumer( IAuthService service ) {
            this._auth = service;
        }

        public async Task Consume( ConsumeContext<PatientUpdated> context ) {

            await _auth.UpdateEmailAsync(new Dtos.UpdateEmailModel {
                Id = context.Message.Id,
                Email = context.Message.Email
            } );
        }
    }
    public class DoctorUpdatedConsumer: IConsumer<DoctorUpdated> {
        private readonly IAuthService _auth;

        public DoctorUpdatedConsumer( IAuthService service ) {
            this._auth = service;
        }

        public async Task Consume( ConsumeContext<DoctorUpdated> context ) {

            await _auth.UpdateEmailAsync(new Dtos.UpdateEmailModel {
                Id = context.Message.Id,
                Email = context.Message.Email
            } );
        }
    }
    public class ReceptionistUpdatedConsumer: IConsumer<ReceptionistUpdated> {
        private readonly IAuthService _auth;

        public ReceptionistUpdatedConsumer( IAuthService service ) {
            this._auth = service;
        }

        public async Task Consume( ConsumeContext<ReceptionistUpdated> context ) {

            await _auth.UpdateEmailAsync(new Dtos.UpdateEmailModel {
                Id = context.Message.Id,
                Email = context.Message.Email
            } );
        }
    }
}
