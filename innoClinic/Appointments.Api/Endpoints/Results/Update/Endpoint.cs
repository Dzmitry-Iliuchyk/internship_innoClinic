using Appointments.Application.Dtos;
using Appointments.Application.Interfaces.Services;
using System.Net;

namespace Result.Update {
    internal sealed class Endpoint: Endpoint<ResultUpdateRequest> {
        public IResultService ResultService { get; set; }
        public override void Configure() {
            Put( "result/update" );
            DontCatchExceptions();
            AllowAnonymous();
            Summary( s => {
                s.Summary = "Used to update an appointment result";
                s.Params[ "ResultUpdateRequest" ] = "Object with data which will be used to upadte an appointment result";
                s.Responses[ (int)HttpStatusCode.NoContent ] = "Returns if successfully updated";
                s.Responses[ (int)HttpStatusCode.NotFound ] = "If the item is not found";
                s.Responses[ (int)HttpStatusCode.BadRequest ] = "If validation is not passed";
            } );
        }

        public override async Task HandleAsync( ResultUpdateRequest r, CancellationToken c ) {
            await ResultService.UpdateAsync( r.Adapt<ResultDto>() );
        }
    }
}