using Appointments.Application.Dtos;
using Appointments.Application.Interfaces.Services;

namespace Result.Update {
    internal sealed class Endpoint: Endpoint<ResultUpdateRequest> {
        public IResultService ResultService { get; set; }
        public override void Configure() {
            Put( "result/update" );
            DontCatchExceptions();
            AllowAnonymous();
        }

        public override async Task HandleAsync( ResultUpdateRequest r, CancellationToken c ) {
            await ResultService.UpdateAsync( r.Adapt<ResultDto>() );
        }
    }
}