using Microsoft.AspNetCore.Mvc;
using Services.Application.Abstractions.Services;
using Services.Application.Abstractions.Services.Dtos;

namespace Services.Api.Controllers {
    [ApiController]
    [Route( "[controller]" )]
    public class ServicesCategoryController: ControllerBase {

        private readonly IServiceCategoryService _service;
        public ServicesCategoryController( IServiceCategoryService service ) {
            this._service = service;
        }

        [HttpGet( "{id:int}/[action]" )]
        public async Task<IResult> Get( int id ) {
            var res = await _service.GetAsync( id );
            return Results.Ok( res );
        }
        [HttpGet( "[action]" )]
        public async Task<IResult> Get() {
            var res = await _service.GetAllAsync();
            return Results.Ok( res );
        }
        [HttpPost( "[action]" )]
        public async Task<IResult> Create( CreateServiceCategoryDto command ) {
            var res = await _service.CreateAsync( command );
            return Results.CreatedAtRoute( value: res );
        }
        [HttpDelete( "{id:int}/[action]" )]
        public async Task<IResult> Delete( int id ) {
            await _service.DeleteAsync( id );
            return Results.NoContent();
        }
        [HttpPut( "[action]" )]
        public async Task<IResult> Update( ServiceCategoryDto command ) {
            await _service.UpdateAsync( command );
            return Results.NoContent();
        }
    }
}
