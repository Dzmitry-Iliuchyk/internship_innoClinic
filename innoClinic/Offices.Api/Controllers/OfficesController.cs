using Microsoft.AspNetCore.Mvc;
using Offices.Api.Middleware;
using Offices.Application.Dtos;
using Offices.Application.Interfaces.Services;

namespace Offices.Api.Controllers {
    [Produces( "application/json" )]
    [Route( "api/[controller]" )]
    [ApiController]
    public class OfficesController: ControllerBase {
        private readonly IOfficeService _officeService;
        public OfficesController( IOfficeService officeService ) {
            this._officeService = officeService;
        }
        /// <summary>
        /// Used to retrieve all offices 
        /// </summary>
        /// <returns>List of offices or empty list if there is not offices</returns>
        /// <response code="200">Returns if successfully performed</response>
        [ProducesResponseType( StatusCodes.Status200OK )]
        [HttpGet( "[action]" )]
        public async Task<IResult> GetOffices() {
            var result = await _officeService.GetAllAsync();
            return Results.Ok( result );
        }
        /// <summary>
        /// Used to retrieve all offices 
        /// </summary>
        /// <returns>List of offices or empty list if there is not offices</returns>
        /// <response code="200">Returns if successfully performed</response>
        [ProducesResponseType( StatusCodes.Status200OK )]
        [HttpGet( "[action]" )]
        public async Task<IResult> GetPage(int skip, int take) {
            var result = await _officeService.GetPageAsync(skip, take);
            return Results.Ok( result );
        }
        /// <summary>
        /// Used to retrieve certain office by id
        /// </summary>
        /// <param name="id">This parameter is identifier of office, represented as objectId as string</param>
        /// <returns >Office</returns>
        /// <response code="200">Returns if successfully performed</response>
        /// <response code="404">If the office not found</response>
        [ProducesResponseType( StatusCodes.Status200OK )]
        [ProducesResponseType( StatusCodes.Status404NotFound, Type = typeof( ErrorResponse ) )]
        [HttpGet( "{id}/[action]" )]
        public async Task<IResult> GetOffice( string id ) {
            var result = await _officeService.GetAsync( id );
            return Results.Ok( result );
        }
        /// <summary>
        /// Used to update info about office
        /// </summary>
        /// <param name="id">Identifier of office which you want to update</param>
        /// <param name="updateOfficeDto">Object with data which replace old one</param>
        /// <returns>Status code only</returns>
        /// <response code="204">Returns if successfully updated</response>
        /// <response code="404">If the item not found</response>
        [ProducesResponseType( StatusCodes.Status204NoContent )]
        [ProducesResponseType( StatusCodes.Status404NotFound, Type = typeof( ErrorResponse ) )]
        [HttpPut( "[action]" )]
        public async Task<IResult> UpdateOffice( string id, UpdateOfficeDto updateOfficeDto ) {
            await _officeService.UpdateAsync( id, updateOfficeDto );
            return Results.NoContent();
        }
        /// <summary>
        /// Used to update path to photo
        /// </summary>
        /// <param name="id">Identifier of office which you want to update</param>
        /// <param name="path">psth to blob</param>
        /// <returns>Status code only</returns>
        /// <response code="201">Returns if successfully updated</response>
        /// <response code="404">If the item not found</response>
        [ProducesResponseType( StatusCodes.Status204NoContent )]
        [ProducesResponseType( StatusCodes.Status404NotFound, Type = typeof( ErrorResponse ) )]
        [HttpPatch( "[action]" )]
        public async Task<IResult> SetPathToOffice( string id, string path ) {
            await _officeService.SetPathToOffice( id, path );
            return Results.NoContent();
        }
        /// <summary>
        /// Used to delete certain office by id
        /// </summary>
        /// <param name="id">This parameter is identifier of office, represented as objectId as string</param>
        /// <returns >StatusCode only</returns>
        /// <response code="204">Returns if successfully performed</response>
        /// <response code="400">If the object cannot pass validation step</response>
        [ProducesResponseType( StatusCodes.Status204NoContent )]
        [ProducesResponseType( StatusCodes.Status400BadRequest, Type = typeof( ErrorResponse ) )]
        [HttpDelete( "[action]" )]
        public async Task<IResult> DeleteOffice( string id ) {
            await _officeService.DeleteAsync( id );
            return Results.NoContent();
        }
        /// <summary>
        /// Creates a new office.
        /// </summary>
        /// <param name="createOfficeDto">Object contains data to office creation </param>
        /// <returns>identifier of new office</returns>
        /// <response code="201">Returns if successfully created</response>
        /// <response code="400">If the object cannot pass validation step</response>
        [ProducesResponseType( StatusCodes.Status201Created )]
        [ProducesResponseType( StatusCodes.Status400BadRequest, Type = typeof( ErrorResponse ) )]
        [HttpPost( "[action]" )]
        public async Task<IResult> CreateOffice( CreateOfficeDto createOfficeDto ) {
            var result = await _officeService.CreateAsync( createOfficeDto );
            return Results.Ok(result);
        }
    }
}
