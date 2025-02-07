using Appointments.Application.Implementations;
using Appointments.Application.Interfaces.Services;
using Result.GetAll;
using System.Net;

namespace GetEmails {
    internal sealed class Endpoint: Endpoint<GetEmailsRequest, GetEmailsResponse> {
        private readonly IAppointmentService _appointments;

        public Endpoint( IAppointmentService appointments ) {
            this._appointments = appointments;
        }

        public override void Configure() {
            Get( "/appointments/{AppointmentId}/getEmails" );
            DontCatchExceptions();
            AllowAnonymous();
            Summary( s => {
                s.Summary = "Used to retrieve emails linked with an appointment";
                s.Params[ "GetEmailsRequest" ] = "Object with id which will be used to retrieve emails";
                s.Responses[ (int)HttpStatusCode.OK ] = "Returns if success";
                s.Responses[ (int)HttpStatusCode.NotFound ] = "If the item is not found";
                s.Responses[ (int)HttpStatusCode.BadRequest ] = "If validation is not passed";
            } );
        }

        public override async Task HandleAsync( GetEmailsRequest r, CancellationToken c ) {
            var res = await _appointments.GetEmailsAsync(r.AppointmentId);
            await SendAsync( new GetEmailsResponse() { Emails = res} );
        }
    }
}