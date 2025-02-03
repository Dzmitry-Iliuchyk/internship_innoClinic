using Authorization.Application.Abstractions.Services;
using MassTransit;
using Shared.Events.Contracts;

namespace Authorization.Application.Consumers {
    public class ProfileDeletedConsumer: IConsumer<ProfileDeleted> {
        private readonly IAuthService _auth;

        public ProfileDeletedConsumer( IAuthService service ) {
            this._auth = service;
        }

        public async Task Consume( ConsumeContext<ProfileDeleted> context ) {
            await _auth.DeleteAccountAsync(id: context.Message.Id);
        }
    }
}
