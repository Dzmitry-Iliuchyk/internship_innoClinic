using Authorization.Application.Abstractions.Services;
using MassTransit;
using Shared.Events.Contracts;

namespace Authorization.Application.Consumers {
    public class ProfileUpdatedConsumer: IConsumer<ProfileUpdated> {
        private readonly IAuthService _auth;

        public ProfileUpdatedConsumer( IAuthService service ) {
            this._auth = service;
        }

        public async Task Consume( ConsumeContext<ProfileUpdated> context ) {

            await _auth.UpdateEmailAsync(new Dtos.UpdateEmailModel {
                Id = context.Message.Id,
                Email = context.Message.Email
            } );
        }
    }
}
