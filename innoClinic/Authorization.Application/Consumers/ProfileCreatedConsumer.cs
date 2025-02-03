using Authorization.Application.Abstractions.Services;
using MassTransit;
using Shared.Events.Contracts;

namespace Authorization.Application.Consumers {
    public class ProfileCreatedConsumer: IConsumer<ProfileCreated> {
        private readonly IAuthService _auth;

        public ProfileCreatedConsumer( IAuthService service ) {
            this._auth = service;
        }

        public async Task Consume( ConsumeContext<ProfileCreated> context ) {
            var pass = Guid.NewGuid().ToString().Substring( 0, 13 );
            await _auth.CreateAccountAsync( new Dtos.CreateAccountModel {
                Id = context.Message.Id,
                Email = context.Message.Email,
                Password = pass,
                Roles = Enum.Parse<Domain.Models.Enums.Roles>( context.Message.Roles.ToString(), true )
            } );
            await context.Send( new SendEmailRequest {
                NameFrom = "AuthService",
                Subject = "YourPassword",
                TextContent = $"<h2> Your default password is: <span>{pass}</span></h2></br><span>Please change your password</span></br></br>If you don't know who send this message, just ignore it.",
                To = [ context.Message.Email ]
            });
        }
    }
}
