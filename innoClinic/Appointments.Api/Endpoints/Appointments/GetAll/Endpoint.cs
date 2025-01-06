using Appointments.Application.Interfaces.Services;


namespace Appointments.GetAll {
    internal sealed class Endpoint: EndpointWithoutRequest<IList<GetAllAppointmentResponse>> {
        private readonly IAppointmentService _appointments;

        public Endpoint( IAppointmentService appointments ) {
            this._appointments = appointments;
        }

        public override void Configure() {
            Get( "Appointments/GetAll" );
            DontCatchExceptions();
            AllowAnonymous();
        }

        public override async Task HandleAsync( CancellationToken c ) {
            Response = ( await _appointments.GetAllAsync() ).Adapt< IList<GetAllAppointmentResponse>>();
        }
    }
}