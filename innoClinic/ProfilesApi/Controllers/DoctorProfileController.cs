using MediatR;
using Microsoft.AspNetCore.Mvc;
using Profiles.Application.Doctors.Commands.Create;
using Profiles.Application.Doctors.Commands.Delete;
using Profiles.Application.Doctors.Commands.Update;
using Profiles.Application.Doctors.Queries.Get;
using Profiles.Application.Doctors.Queries.GetFiltered;
using Profiles.Application.Doctors.Queries.GetPage;

namespace ProfilesApi.Controllers {
    [ApiController]
    [Route( "[controller]" )]
    public class DoctorProfileController: ControllerBase {
       
        private readonly ISender _sender;
        public DoctorProfileController(ISender sender ) {
            _sender = sender;
        }

        [HttpGet( "{id:guid}/[action]" )]
        public async Task<IResult> Get( Guid id ) {
            var res = await _sender.Send( new GetDoctorQuery( id ) );
            return Results.Ok( res );
        }
        [HttpGet( "[action]" )]
        public async Task<IResult> Get( ) {
            var res = await _sender.Send( new GetDoctorsQuery() );
            return Results.Ok( res );
        }
        [HttpGet( "[action]" )]
        public async Task<IResult> GetPaged(int skip, int take ) {
            var res = await _sender.Send( new GetPagedDoctorsQuery(skip, take) );
            return Results.Ok( res );
        }
        [HttpPost( "[action]" )]
        public async Task<IResult> Create( CreateDoctorCommand command ) {
            var res = await _sender.Send( command );
            return Results.CreatedAtRoute( value: res );
        }
        [HttpDelete( "{doctorId:guid}/[action]" )]
        public async Task<IResult> Delete( Guid doctorId ) {
            await _sender.Send( new DeleteDoctorCommand(doctorId) );
            return Results.NoContent( );
        }
        [HttpPut( "[action]" )]
        public async Task<IResult> Update( UpdateDoctorCommand command ) {
            await _sender.Send( command );
            return Results.NoContent( );
        }
    }
}
