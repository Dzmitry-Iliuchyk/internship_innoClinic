using MediatR;
using Microsoft.AspNetCore.Mvc;
using Profiles.Application.Patients.Commands.Create;
using Profiles.Application.Patients.Commands.Delete;
using Profiles.Application.Patients.Commands.Update;
using Profiles.Application.Patients.Queries.Get;
using Profiles.Application.Patients.Queries.GetFiltered;

namespace ProfilesApi.Controllers {
    [ApiController]
    [Route( "[controller]" )]
    public class PatientProfileController: ControllerBase {

        private readonly ISender _sender;
        public PatientProfileController( ISender sender ) {
            _sender = sender;
        }

        [HttpGet( "{id:guid}/[action]" )]
        public async Task<IResult> Get( Guid id ) {
            var res = await _sender.Send( new GetPatientQuery( id ) );
            return Results.Ok( res );
        }
        [HttpGet( "[action]" )]
        public async Task<IResult> Get() {
            var res = await _sender.Send( new GetPatientsQuery() );
            return Results.Ok( res );
        }
        [HttpPost( "[action]" )]
        public async Task<IResult> Create( CreatePatientCommand command ) {
            var res = await _sender.Send( command );
            return Results.CreatedAtRoute( value: res );
        }
        [HttpDelete( "{patientId:guid}/[action]" )]
        public async Task<IResult> Delete( Guid patientId ) {
            await _sender.Send( new DeletePatientCommand( patientId ) );
            return Results.NoContent();
        }
        [HttpPut( "[action]" )]
        public async Task<IResult> Update( UpdatePatientCommand command ) {
            await _sender.Send( command );
            return Results.NoContent();
        }
    }
}
