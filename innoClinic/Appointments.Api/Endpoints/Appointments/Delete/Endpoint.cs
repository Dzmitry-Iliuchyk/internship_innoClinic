using Appointments.Application.Interfaces.Services;

namespace Appointments {
    internal sealed class Endpoint: Endpoint<DeleteAppointmentRequest> {
        public required IAppointmentService Appointments { get; set; }
        public override void Configure() {
            Post( "Appointments/Delete" );
            DontCatchExceptions();
            AllowAnonymous();
        }

        public override async Task HandleAsync( DeleteAppointmentRequest r, CancellationToken c ) {
            await Appointments.DeleteAsync(r.Id);
        }
    }
}