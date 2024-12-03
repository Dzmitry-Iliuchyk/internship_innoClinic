using Microsoft.AspNetCore.Mvc;
using Offices.Application.Dtos;
using Offices.Application.Interfaces.Services;

namespace Offices.Api.Controllers {
    [Route( "api/[controller]" )]
    [ApiController]
    public class OfficesController: ControllerBase {
        private readonly IOfficeService _officeService;
        public OfficesController( IOfficeService officeService ) {
            this._officeService = officeService;
        }
        [HttpGet( "[action]" )]
        public async Task<IResult> GetOffices() {
            var result = await _officeService.GetAllAsync();
            return Results.Ok( result );
        }
        [HttpGet( "{id}/[action]" )]
        public async Task<IResult> GetOffice( string id ) {
            var result = await _officeService.GetAsync( id );
            return Results.Ok( result );
        }
        [HttpPut( "[action]" )]
        public async Task<IResult> UpdateOffice( string id, UpdateOfficeDto updateOfficeDto ) {
            await _officeService.UpdateAsync( id, updateOfficeDto );
            return Results.NoContent();
        }
        [HttpDelete( "[action]" )]
        public async Task<IResult> DeleteOffice( string id ) {
            await _officeService.DeleteAsync( id );
            return Results.NoContent();
        }
        [HttpPost( "[action]" )]
        public async Task<IResult> CreateOffice( CreateOfficeDto createOfficeDto ) {
            var result = await _officeService.CreateAsync( createOfficeDto );
            return Results.Ok(result);
        }
    }
}
