using MassTransit;
using MediatR;
using Profiles.Application.Doctors.Commands.Create;
using Profiles.Application.Doctors.Commands.Delete;
using Profiles.Application.Doctors.Commands.Update;
using Profiles.Application.Patients.Commands.Create;
using Profiles.Application.Patients.Commands.Delete;
using Profiles.Application.Patients.Commands.Update;
using Profiles.Application.Receptionists.Commands.Create;
using Profiles.Application.Receptionists.Commands.Delete;
using Profiles.Application.Receptionists.Commands.Update;
using Shared.Events.Contracts;
using System.Reflection;

namespace Profiles.Application.Common.Behavior {
    public class MassTransitPipelineBehavior<TRequest, TResponse>: IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse> {
        private readonly IPublishEndpoint _publishEndpoint;
        private static Type[] DeleteCommands = [
            typeof(DeletePatientCommand),
            typeof(DeleteDoctorCommand),
            typeof(DeleteReceptionistCommand),
            ];
        public MassTransitPipelineBehavior( IPublishEndpoint publishEndpoint ) {
            _publishEndpoint = publishEndpoint;
        }

        public async Task<TResponse> Handle( TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken ) {

            var response = await next();
           

            switch (request) {
                case var req when DeleteCommands.Contains(req.GetType()):
                    var personId = (Guid)request.GetType()?.GetProperties()?.First()?.GetValue(request);
                    await _publishEndpoint.Publish( new ProfileDeleted() {
                            Id = personId
                        }, 
                        x => x.Durable = true,
                        cancellationToken );
                    break;

                case UpdatePersonCommandBase updatePerson:
                    
                    await _publishEndpoint.Publish( new ProfileUpdated() {
                        Email = updatePerson.Email,
                        Id = updatePerson.Id },
                        x => x.Durable = true,
                        cancellationToken );
                    break;

                case CreatePersonCommandBase createPerson:

                    if (response is Guid id) {
                        await _publishEndpoint.Publish( new ProfileCreated() {
                            Id = id,
                            Email = createPerson.Email,
                            Roles = request switch {
                                CreateDoctorCommand => Roles.Doctor,
                                CreatePatientCommand => Roles.Patient,
                                CreateReceptionistCommand => Roles.Receptionist,
                                _ => throw new NotImplementedException()
                            }
                        },
                        x => x.Durable = true,
                        cancellationToken );
                    }
                    break;
            }

            return response;
        }
    }
}
