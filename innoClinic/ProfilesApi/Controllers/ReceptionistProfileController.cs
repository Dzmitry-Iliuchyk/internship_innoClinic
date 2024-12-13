using MediatR;
using Microsoft.AspNetCore.Mvc;
using Profiles.Application.Receptionists.Commands.Create;
using Profiles.Application.Receptionists.Commands.Delete;
using Profiles.Application.Receptionists.Commands.Update;
using Profiles.Application.Receptionists.Queries.Get;
using Profiles.Application.Receptionists.Queries.GetFiltered;

namespace ProfilesApi.Controllers {
    [ApiController]
    [Route( "[controller]" )]
    public class ReceptionistProfileController: ControllerBase {

        private readonly ISender _sender;
        public ReceptionistProfileController( ISender sender ) {
            _sender = sender;
        }

        [HttpGet( "{id:guid}/[action]" )]
        public async Task<IResult> Get( Guid id ) {
            var res = await _sender.Send( new GetReceptionistQuery( id ) );
            return Results.Ok( res );
        }
        [HttpGet( "[action]" )]
        public async Task<IResult> Get() {
            var res = await _sender.Send( new GetReceptionistsQuery() );
            return Results.Ok( res );
        }
        [HttpPost( "[action]" )]
        public async Task<IResult> Create( CreateReceptionistCommand command ) {
            var res = await _sender.Send( command );
            return Results.CreatedAtRoute( value: res );
        }
        [HttpDelete( "{receptionistId:guid}/[action]" )]
        public async Task<IResult> Delete( Guid receptionistId ) {
            await _sender.Send( new DeleteReceptionistCommand( receptionistId ) );
            return Results.NoContent();
        }
        [HttpPut( "[action]" )]
        public async Task<IResult> Update( UpdateReceptionistCommand command ) {
            await _sender.Send( command );
            return Results.NoContent();
        }
    }
}
