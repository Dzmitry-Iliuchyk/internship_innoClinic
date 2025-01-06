using Appointments.Application.Interfaces.Services;
using System.Net;

namespace Result.Get {
    internal sealed class Endpoint: Endpoint<ResultGetRequest, ResultGetResponse> {
        public IResultService ResultService { get; set; }
        public override void Configure() {
            Get( "result/{id:guid}/get" );
            DontCatchExceptions();
            AllowAnonymous();
            Summary( s => {
                s.Summary = "Used to get an appointment result";
                s.Params[ "ResultGetRequest" ] = "Object with data which will be used to retrieve an appointment result";
                s.Responses[ (int)HttpStatusCode.OK ] = "Returns if successfully retrieved";
                s.Responses[ (int)HttpStatusCode.NotFound ] = "If the item is not found";
                s.Responses[ (int)HttpStatusCode.BadRequest ] = "If validation is not passed";
            } );
        }

        public override async Task HandleAsync( ResultGetRequest r, CancellationToken c ) {
            var result = await ResultService.GetAsync(r.Id);
            await SendAsync( result.Adapt<ResultGetResponse>() );
        }
    }
}