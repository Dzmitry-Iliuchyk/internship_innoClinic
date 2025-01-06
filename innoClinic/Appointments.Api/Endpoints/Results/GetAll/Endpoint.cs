using Appointments.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;

namespace Result.GetAll {
    [Authorize(Roles = "admin")]
    internal sealed class Endpoint: EndpointWithoutRequest<IList<ResultGetAllResponse>> {
        public IResultService ResultService { get; set; }
        public override void Configure() {
            Get( "result/getAll" );
            DontCatchExceptions();
        }

        public override async Task HandleAsync( CancellationToken c ) {
            var res = await ResultService.GetAllAsync();
            await SendAsync( res.Adapt<IList<ResultGetAllResponse>>() );
        }
    }
}