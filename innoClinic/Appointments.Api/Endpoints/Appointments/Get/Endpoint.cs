using Appointments.Application.Interfaces.Services;
using System.Net;

namespace Appointments.Get {
    public class Endpoint: Endpoint<GetAppointmentRequest, GetAppointmentResponse> {
        private readonly IAppointmentService _appointments ;

        public Endpoint( IAppointmentService appointments ) {
            this._appointments = appointments;
        }

        public override void Configure() {
            Get( "/appointments/{AppointmentId}/get");
            DontCatchExceptions();
            AllowAnonymous();
            Summary( s => {
                s.Summary = "Used to retrieve new appointment";
                s.Params[ "GetAppointmentRequest" ] = "Object with data which will be used to retrieve appointment";
                s.Responses[ (int)HttpStatusCode.OK ] = "Returns if successfully created";
                s.Responses[ (int)HttpStatusCode.NotFound ] = "If the item is not found";
                s.Responses[ (int)HttpStatusCode.BadRequest ] = "If validation is not passed";
            } );
        }

        public override async Task HandleAsync( GetAppointmentRequest r, CancellationToken c ) {
            Response = (await _appointments.GetAsync(Guid.Parse(r.AppointmentId ) )).Adapt<GetAppointmentResponse>();
        }
    }
}