using Appointments.Application.Dtos;
using Appointments.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using System.Net;

namespace Appointments.Update {
    internal sealed class Endpoint: Endpoint<UpdateAppointmentRequest> {
        public IAppointmentService Appointments {  get; set; }
        public override void Configure() {
            Put( "appointments/update" );
            DontCatchExceptions();
            AllowAnonymous();
            Summary( s => {
                s.Summary = "Used to update an appointment";
                s.Params[ "UpdateAppointmentRequest" ] = "Object with data which will be used to update an appointment";
                s.Responses[ (int)HttpStatusCode.OK ] = "Returns if successfully updated";
                s.Responses[ (int)HttpStatusCode.NotFound ] = "If the item is not found";
                s.Responses[ (int)HttpStatusCode.BadRequest ] = "If validation is not passed";
            } );
        }

        public override async Task HandleAsync( UpdateAppointmentRequest r, CancellationToken c ) {
            await Appointments.UpdateAsync(r.Adapt<AppointmentUpdateDto>());
        }
    }
}