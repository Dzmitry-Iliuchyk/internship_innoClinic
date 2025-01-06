using Appointments.Application.Interfaces.Services;
using System.Net;

namespace Appointments {
    internal sealed class Endpoint: Endpoint<DeleteAppointmentRequest> {
        public required IAppointmentService Appointments { get; set; }
        public override void Configure() {
            Delete( "appointments/{Id}/delete" );
            DontCatchExceptions();
            AllowAnonymous();
            Summary( s => {
                s.Summary = "Used to delete an appointment";
                s.Params[ "DeleteAppointmentRequest" ] = "Contains identifier of the appointment to delete";
                s.Responses[ (int)HttpStatusCode.NoContent ] = "Returns if successfully deleted";
                s.Responses[ (int)HttpStatusCode.NotFound ] = "If the item is not found";
                s.Responses[ (int)HttpStatusCode.BadRequest ] = "If validation is not passed";
            } );
        }

        public override async Task HandleAsync( DeleteAppointmentRequest r, CancellationToken c ) {
            await Appointments.DeleteAsync(r.Id);
        }
    }
}