using Appointments.Application.Interfaces.Services;

namespace Appointments.Get {
    public class Endpoint: Endpoint<GetAppointmentRequest, GetAppointmentResponse> {
        private readonly IAppointmentService _appointments ;

        public Endpoint( IAppointmentService appointments ) {
            this._appointments = appointments;
        }

        public override void Configure() {
            Get("/appointments/get/{@AppointmentId}" , x => new { x.AppointmentId} );
            DontCatchExceptions();
            AllowAnonymous();
           
        }

        public override async Task HandleAsync( GetAppointmentRequest r, CancellationToken c ) {
            Response = (await _appointments.GetAsync(Guid.Parse(r.AppointmentId ) )).Adapt<GetAppointmentResponse>();
        }
    }
}