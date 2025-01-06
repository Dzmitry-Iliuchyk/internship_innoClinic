using Appointments.Application.Dtos;
using Appointments.Application.Interfaces.Services;
using System.Net;

namespace Appointments.Create {
    internal sealed class Endpoint: Endpoint<CreateAppointmentRequest, CreateAppointmentResponse> {
        public IAppointmentService Appointment { get; set; }
        public override void Configure() {
            Post( "Appointments/Create" );
            DontCatchExceptions();
            AllowAnonymous();
        }

        public override async Task HandleAsync( CreateAppointmentRequest r, CancellationToken c ) {
            await this.SendAsync(new() { Id = await Appointment.CreateAsync( r.Adapt<AppointmentCreateDto>() ) },statusCode: ((int)HttpStatusCode.Created),cancellation: c);
        }
    }
}