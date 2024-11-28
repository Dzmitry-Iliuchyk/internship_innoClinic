using Authorization.Application.Abstractions.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Authorization.Api.Controllers {
    [Route( "api/[controller]" )]
    [ApiController]
    public class AuthController: ControllerBase {
        private readonly IAuthService _auth;

        public AuthController(IAuthService authService) {
            _auth = authService;
        }

        [HttpPost( "[action]" )]
        public async Task<IResult> SignUp(string email, string password) {
            await _auth.SignUpAsync(new Application.Dtos.SignUpModel { Email = email, Password = password } );
            return Results.Ok();
        }
        [HttpPost( "[action]" )]
        public async Task<IResult> SignIn( string email, string password ) {
            var token = await _auth.SignInAsync( new Application.Dtos.SignInModel { Email = email, Password = password } );
            return Results.Ok(token);
        }
        [HttpPost( "[action]" )]
        public async Task<IResult> AddRoleToUser(Guid userId, int roleId) {
            await _auth.AddRoleToUserAsync(userId, roleId);
            return Results.Ok();
        }
        [HttpPost( "[action]" )]
        public async Task<IResult> RemoveRoleFromUser( Guid userId, int roleId ) {
            await _auth.RemoveRoleFromUserAsync( userId, roleId );
            return Results.Ok();
        }
        [HttpGet( "[action]" )]
        public async Task<IResult> GetRoles( Guid userId ) {
            var roles = await _auth.GetRoles( userId );
            return Results.Ok(roles);
        }
        [HttpGet( "[action]" )]
        public async Task<IResult> GetUsers( ) {
            var users = await _auth.GetUsers( );
            return Results.Ok(users);
        }
        [HttpGet( "[action]" )]
        public async Task<IResult> GetUserWithRoles( Guid userId ) {
            var user = await _auth.GetUserWithRoles( userId );
            return Results.Ok(user);
        }

    }
}
