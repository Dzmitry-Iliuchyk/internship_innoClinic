using Appointments.Application.Dtos;
using Appointments.Application.Interfaces.Services;

namespace Result.Create {
    internal sealed class Endpoint: Endpoint<ResultCreateRequest, ResultCreateResponse> {
        public IResultService ResultService { get; set; }
        public override void Configure() {
            Post( "result/create" );
            DontCatchExceptions();
            AllowAnonymous();
        }

        public override async Task HandleAsync(ResultCreateRequest r, CancellationToken c ) {
            var res = await ResultService.CreateAsync(r.Adapt<ResultCreateDto>());
            await SendAsync( new ResultCreateResponse() {Id= res } );
        }
    }
}