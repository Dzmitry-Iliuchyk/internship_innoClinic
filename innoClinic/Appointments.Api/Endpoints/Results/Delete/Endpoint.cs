using Appointments.Application.Interfaces.Services;

namespace Result.Delete {
    internal sealed class Endpoint: Endpoint<ResultDeleteRequest> {
        public IResultService ResultService { get; set; }
        public override void Configure() {
            Delete( "result/{id:guid}/delete" );
            DontCatchExceptions();
            AllowAnonymous();
        }

        public override async Task HandleAsync( ResultDeleteRequest r, CancellationToken c ) {
            await ResultService.DeleteAsync(r.Id);
        }
    }
}