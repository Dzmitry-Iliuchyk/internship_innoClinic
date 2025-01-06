using Appointments.Application.Dtos;
using Appointments.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;

namespace Appointmeents.Update {
    internal sealed class Endpoint: Endpoint<UpdateAppointmentRequest> {
        public IAppointmentService Appointments {  get; set; }
        public override void Configure() {
            Put( "Appointments/Update" );
            DontCatchExceptions();
            AllowAnonymous();
        }

        public override async Task HandleAsync( UpdateAppointmentRequest r, CancellationToken c ) {
            await Appointments.UpdateAsync(r.Adapt<AppointmentUpdateDto>());
        }
    }
}