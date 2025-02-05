using Authorization.Application.Abstractions.Services;
using MassTransit;
using Shared.Events.Contracts;

namespace Authorization.Application.Consumers {
    public class PatientDeletedConsumer: IConsumer<PatientDeleted> {
        private readonly IAuthService _auth;

        public PatientDeletedConsumer( IAuthService service ) {
            this._auth = service;
        }

        public async Task Consume( ConsumeContext<PatientDeleted> context ) {
            await _auth.DeleteAccountAsync(id: context.Message.Id);
        }
    }
    public class DoctorDeletedConsumer: IConsumer<DoctorDeleted> {
        private readonly IAuthService _auth;

        public DoctorDeletedConsumer( IAuthService service ) {
            this._auth = service;
        }

        public async Task Consume( ConsumeContext<DoctorDeleted> context ) {
            await _auth.DeleteAccountAsync(id: context.Message.Id);
        }
    }
    public class ReceptionistDeletedConsumer: IConsumer<ReceptionistDeleted> {
        private readonly IAuthService _auth;

        public ReceptionistDeletedConsumer( IAuthService service ) {
            this._auth = service;
        }

        public async Task Consume( ConsumeContext<ReceptionistDeleted> context ) {
            await _auth.DeleteAccountAsync(id: context.Message.Id);
        }
    }
}
