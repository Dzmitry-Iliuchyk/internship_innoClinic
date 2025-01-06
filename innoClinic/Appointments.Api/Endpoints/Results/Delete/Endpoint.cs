using Appointments.Application.Interfaces.Services;
using System.Net;

namespace Result.Delete {
    internal sealed class Endpoint: Endpoint<ResultDeleteRequest> {
        public IResultService ResultService { get; set; }
        public override void Configure() {
            Delete( "result/{id:guid}/delete" );
            DontCatchExceptions();
            AllowAnonymous();
            Summary( s => {
                s.Summary = "Used to delete appointment result";
                s.Params[ "ResultDeleteRequest" ] = "Object with data which will be used to delete an appointment result";
                s.Responses[ (int)HttpStatusCode.NoContent ] = "Returns if successfully created";
                s.Responses[ (int)HttpStatusCode.NotFound ] = "If the item is not found";
                s.Responses[ (int)HttpStatusCode.BadRequest ] = "If validation is not passed";
            } );
        }

        public override async Task HandleAsync( ResultDeleteRequest r, CancellationToken c ) {
            await ResultService.DeleteAsync(r.Id);
        }
    }
}