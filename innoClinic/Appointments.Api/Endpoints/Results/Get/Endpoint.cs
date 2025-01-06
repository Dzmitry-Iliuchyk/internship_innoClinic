using Appointments.Application.Interfaces.Services;

namespace Result.Get {
    internal sealed class Endpoint: Endpoint<ResultGetRequest, ResultGetResponse> {
        public IResultService ResultService { get; set; }
        public override void Configure() {
            Get( "result/{id:guid}/get" );
            DontCatchExceptions();
        }

        public override async Task HandleAsync( ResultGetRequest r, CancellationToken c ) {
            var result = await ResultService.GetAsync(r.Id);
            await SendAsync( result.Adapt<ResultGetResponse>() );
        }
    }
}