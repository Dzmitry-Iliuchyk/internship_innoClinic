using Appointments.Application.Interfaces.Services;
using System.Net;


namespace Appointments.GetAll {
    internal sealed class Endpoint: EndpointWithoutRequest<IList<GetAllAppointmentResponse>> {
        private readonly IAppointmentService _appointments;

        public Endpoint( IAppointmentService appointments ) {
            this._appointments = appointments;
        }

        public override void Configure() {
            Get( "appointments/getAll" );
            DontCatchExceptions();
            AllowAnonymous();
            Summary( s => {
                s.Summary = "Used to retrieve a list of appointments";
                s.Responses[ (int)HttpStatusCode.OK ] = "Returns if successfully retrieved";
            } );
        }

        public override async Task HandleAsync( CancellationToken c ) {
            Response = ( await _appointments.GetAllAsync() ).Adapt< IList<GetAllAppointmentResponse>>();
        }
    }
}