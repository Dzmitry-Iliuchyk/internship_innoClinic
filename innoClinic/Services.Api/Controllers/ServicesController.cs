using Microsoft.AspNetCore.Mvc;
using Services.Application.Abstractions.Services;
using Services.Application.Abstractions.Services.Dtos;

namespace Services.Api.Controllers {
    [ApiController]
    [Route( "[controller]" )]
    public class ServicesController: ControllerBase {

        private readonly IServiceService _service;
        public ServicesController( IServiceService service ) {
            this._service = service;
        }

        [HttpGet( "{id:guid}/[action]" )]
        public async Task<IResult> Get( Guid id ) {
            var res = await _service.GetAsync( id );
            return Results.Ok( res );
        }
        [HttpGet( "[action]" )]
        public async Task<IResult> Get() {
            var res = await _service.GetAllAsync();
            return Results.Ok( res );
        }

        [HttpGet( "[action]" )]
        public async Task<IResult> GetPage(int skip, int take) {
            var res = await _service.GetPageAsync(skip, take);
            return Results.Ok( res );
        }
        [HttpPost( "[action]" )]
        public async Task<IResult> Create( CreateServiceDto command ) {
            var res = await _service.CreateAsync( command );
            return Results.CreatedAtRoute( value: res );
        }
        [HttpDelete( "{id:guid}/[action]" )]
        public async Task<IResult> Delete( Guid id ) {
            await _service.DeleteAsync( id );
            return Results.NoContent();
        }
        [HttpPut( "[action]" )]
        public async Task<IResult> Update( UpdateServiceDto command ) {
            await _service.UpdateAsync( command );
            return Results.NoContent();
        }
    }
}
