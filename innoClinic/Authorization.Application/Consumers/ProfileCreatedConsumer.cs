using Authorization.Application.Abstractions.Services;
using MassTransit;
using Shared.Events.Contracts;
using Shared.Events.Contracts.ProfilesMessages;

namespace Authorization.Application.Consumers {
    public class PatientCreatedConsumer: IConsumer<PatientCreated> {
        private readonly IAuthService _auth;

        public PatientCreatedConsumer( IAuthService service ) {
            this._auth = service;
        }

        public async Task Consume( ConsumeContext<PatientCreated> context ) {
            var pass = Guid.NewGuid().ToString().Substring( 0, 13 );
            await _auth.CreateAccountAsync( new Dtos.CreateAccountModel {
                Id = context.Message.Id,
                Email = context.Message.Email,
                Password = pass,
                Roles = Domain.Models.Enums.Roles.Patient
            } );

            await context.Publish( new SendEmailRequest {
                NameFrom = "AuthService",
                Subject = "YourPassword",
                TextContent = $"<h2> Your default password is: <span>{pass}</span></h2></br><span>Please change your password</span></br></br>If you don't know who send this message, just ignore it.",
                To = [ context.Message.Email ]
            });
        }
    }
    public class DoctorCreatedConsumer: IConsumer<DoctorCreated> {
        private readonly IAuthService _auth;

        public DoctorCreatedConsumer( IAuthService service ) {
            this._auth = service;
        }

        public async Task Consume( ConsumeContext<DoctorCreated> context ) {
            var pass = Guid.NewGuid().ToString().Substring( 0, 13 );
            await _auth.CreateAccountAsync( new Dtos.CreateAccountModel {
                Id = context.Message.Id,
                Email = context.Message.Email,
                Password = pass,
                Roles = Domain.Models.Enums.Roles.Doctor
            } );

            await context.Publish( new SendEmailRequest {
                NameFrom = "AuthService",
                Subject = "YourPassword",
                TextContent = $"<h2> Your default password is: <span>{pass}</span></h2></br><span>Please change your password</span></br></br>If you don't know who send this message, just ignore it.",
                To = [ context.Message.Email ]
            });
        }
    }
    public class ReceptionistCreatedConsumer: IConsumer<ReceptionistCreated> {
        private readonly IAuthService _auth;

        public ReceptionistCreatedConsumer( IAuthService service ) {
            this._auth = service;
        }

        public async Task Consume( ConsumeContext<ReceptionistCreated> context ) {
            var pass = Guid.NewGuid().ToString().Substring( 0, 13 );
            await _auth.CreateAccountAsync( new Dtos.CreateAccountModel {
                Id = context.Message.Id,
                Email = context.Message.Email,
                Password = pass,
                Roles = Domain.Models.Enums.Roles.Receptionist
            } );

            await context.Publish( new SendEmailRequest {
                NameFrom = "AuthService",
                Subject = "YourPassword",
                TextContent = $"<h2> Your default password is: <span>{pass}</span></h2></br><span>Please change your password</span></br></br>If you don't know who send this message, just ignore it.",
                To = [ context.Message.Email ]
            });
        }
    }
}
