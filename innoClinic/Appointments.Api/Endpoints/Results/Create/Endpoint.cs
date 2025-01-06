using Appointments.Application.Dtos;
using Appointments.Application.Interfaces.Services;
using System.Net;

namespace Result.Create {
    internal sealed class Endpoint: Endpoint<ResultCreateRequest, ResultCreateResponse> {
        public IResultService ResultService { get; set; }
        public override void Configure() {
            Post( "result/create" );
            DontCatchExceptions();
            AllowAnonymous();
            Summary( s => {
                s.Summary = "Used to create new appointment result";
                s.Params[ "ResultCreateRequest" ] = "Object with data which will be used to create a new appointment result";
                s.Responses[ (int)HttpStatusCode.Created ] = "Returns if successfully created";
                s.Responses[ (int)HttpStatusCode.NotFound ] = "If the item is not found";
                s.Responses[ (int)HttpStatusCode.BadRequest ] = "If validation is not passed";
            } );
        }

        public override async Task HandleAsync(ResultCreateRequest r, CancellationToken c ) {
            var res = await ResultService.CreateAsync(r.Adapt<ResultCreateDto>());
            await SendAsync( new ResultCreateResponse() {Id= res } );
        }
    }
}