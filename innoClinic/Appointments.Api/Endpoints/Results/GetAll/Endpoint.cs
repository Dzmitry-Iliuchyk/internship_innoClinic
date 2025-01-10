using Appointments.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using System.Net;

namespace Result.GetAll {
    [Authorize(Roles = "admin")]
    internal sealed class Endpoint: EndpointWithoutRequest<IList<ResultGetAllResponse>> {
        public IResultService ResultService { get; set; }
        public override void Configure() {
            Get( "result/getAll" );
            DontCatchExceptions();
            Summary( s => {
                s.Summary = "Used to get all results";
                s.Responses[ (int)HttpStatusCode.OK ] = "Returns if successfully retrieved";
                s.Responses[ (int)HttpStatusCode.NotFound ] = "If the item is not found";
                s.Responses[ (int)HttpStatusCode.BadRequest ] = "If validation is not passed";
            } );
        }

        public override async Task HandleAsync( CancellationToken c ) {
            var res = await ResultService.GetAllAsync();
            await SendAsync( res.Adapt<IList<ResultGetAllResponse>>() );
        }
    }
}