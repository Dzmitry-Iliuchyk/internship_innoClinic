using Appointments.Application.Dtos;
using Appointments.Application.Interfaces.Services;
using System.Net;

namespace Appointments.Create {
    internal sealed class Endpoint: Endpoint<CreateAppointmentRequest, CreateAppointmentResponse> {
        public IAppointmentService Appointment { get; set; }
        public override void Configure() {
            Post( "appointments/create" );
            DontCatchExceptions();
            AllowAnonymous();
            Summary( s => { 
                s.Summary = "Used to create new appointment";
                s.Params[ "CreateAppointmentRequest" ] = "Object with data which will be used to create a new appointment";
                s.Responses[ (int)HttpStatusCode.Created ] = "Returns if successfully created";
                s.Responses[ (int)HttpStatusCode.NotFound ] = "If the item is not found"; 
                s.Responses[ (int)HttpStatusCode.BadRequest ] = "If validation is not passed"; 
            } );
        }

        public override async Task HandleAsync( CreateAppointmentRequest r, CancellationToken c ) {
            await this.SendAsync(new() { Id = await Appointment.CreateAsync( r.Adapt<AppointmentCreateDto>() ) },statusCode: ((int)HttpStatusCode.Created),cancellation: c);
        }
    }
}