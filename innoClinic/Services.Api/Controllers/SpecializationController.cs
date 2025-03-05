using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Application.Abstractions.Services.Dtos;
using Services.Application.Abstractions.Services;

namespace Services.Api.Controllers {
    [Route( "api/[controller]" )]
    [ApiController]
    public class SpecializationController: ControllerBase {
        private readonly ISpecializationService _service;
        public SpecializationController( ISpecializationService service ) {
            this._service = service;
        }

        [HttpGet( "{id:int}/[action]" )]
        public async Task<IResult> Get( int id ) {
            var res = await _service.GetAsync( id );
            return Results.Ok( res );
        }
        [HttpGet( "[action]" )]
        public async Task<IResult> GetNames() {
            var res = await _service.GetAllNamesAsync();
            return Results.Ok( res );
        }
        [HttpPost( "[action]" )]
        public async Task<IResult> Create( CreateSpecializationDto command ) {
            var res = await _service.CreateAsync( command );
            return Results.CreatedAtRoute( value: res );
        }
        [HttpDelete( "{id:int}/[action]" )]
        public async Task<IResult> Delete( int id ) {
            await _service.DeleteAsync( id );
            return Results.NoContent();
        }
        [HttpPut( "[action]" )]
        public async Task<IResult> Update( SpecializationDto command ) {
            await _service.UpdateAsync( command );
            return Results.NoContent();
        }
    }
}
