using MediatR;
using Microsoft.AspNetCore.Mvc;
using Profiles.Application.Utilities.Commands.SetImagePathToAccount;

namespace ProfilesApi.Controllers {
    [ApiController]
    [Route( "[controller]" )]
    public class UtilityController: ControllerBase {

        private readonly ILogger<UtilityController> _logger;
        private readonly ISender _sender;

        public UtilityController( ILogger<UtilityController> logger, ISender sender ) {
            _logger = logger;
            this._sender = sender;
        }

        [HttpPatch( "[action]" )]
        public async Task<IResult> SetImagePath( Guid id, string path ) {
            await _sender.Send( new SetImagePathCommand( id, path ) );
            return Results.NoContent();
        }
        [HttpGet( "[action]" )]
        public async Task<IResult> GetImagePath( Guid id ) {
            var path = await _sender.Send( new GetImagePathRequest( id ) );
            return Results.Ok(path);
        }
    }
}
