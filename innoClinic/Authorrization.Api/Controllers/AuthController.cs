using Authorization.Api.Dto;
using Authorization.Api.Middleware;
using Authorization.Application.Abstractions.Services;
using Microsoft.AspNetCore.Mvc;

namespace Authorization.Api.Controllers {
    [Produces( "application/json" )]
    [Route( "api/[controller]" )]
    [ApiController]
    public class AuthController: ControllerBase {
        private readonly IAuthService _auth;

        public AuthController( IAuthService authService ) {
            _auth = authService;
        }
        /// <summary>
        /// Creates a new User.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns>Nothing</returns>
        /// <response code="201">Returns if successfully created</response>
        /// <response code="400">If the item already exists</response>
        [HttpPost( "[action]" )]
        [ProducesResponseType( StatusCodes.Status201Created )]
        [ProducesResponseType( StatusCodes.Status400BadRequest, Type = typeof( ErrorResponse ) )]
        public async Task<IResult> SignUp( string email, string password ) {
            await _auth.SignUpAsync( new Application.Dtos.SignUpModel { Email = email, Password = password } );
            return Results.Created();
        }
        /// <summary>
        /// Used to obtain access token to application.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns>Access Token</returns>
        /// <response code="200">Returns access token if signIn process successful</response>
        /// <response code="400">If wrong password has entered</response>
        /// <response code="404">If user doesn't exist </response>
        [HttpPost( "[action]" )]
        [ProducesResponseType( StatusCodes.Status200OK )]
        [ProducesResponseType( StatusCodes.Status400BadRequest, Type = typeof( ErrorResponse ) )]
        [ProducesResponseType( StatusCodes.Status404NotFound, Type = typeof( ErrorResponse ) )]
        public async Task<IResult> SignIn( string email, string password ) {
            var token = await _auth.SignInAsync( new Application.Dtos.SignInModel { Email = email, Password = password } );
            return Results.Ok( token );
        }
        /// <summary>
        /// Used to update data about user.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Nothing</returns>
        /// <response code="204">Returns if updated successfully</response
        /// <response code="404">If user doesn't exist </response>
        [HttpPut( "[action]" )]
        [ProducesResponseType( StatusCodes.Status204NoContent )]
        [ProducesResponseType( StatusCodes.Status400BadRequest, Type = typeof( ErrorResponse ) )]
        [ProducesResponseType( StatusCodes.Status404NotFound, Type = typeof( ErrorResponse ) )]
        public async Task<IResult> Update( UpdateRequest request ) {
            await _auth.UpdateAsync( new Application.Dtos.UpdateModel { Email = request.Email, Password = request.Password, Id = request.Id } );
            return Results.NoContent();
        }
        /// <summary>
        /// Used to assign the role to the user.
        /// </summary>
        /// <param name="userId">Id of the user</param>
        /// <param name="roleId"> Id of tre role</param>
        /// <returns>Nothing</returns>
        /// <response code="200">If the role successfully assigned to tha user </response>
        /// <response code="400">If the role already assigned</response>
        /// <response code="404">If user doesn't exist </response>
        [HttpPost( "[action]" )]
        [ProducesResponseType( StatusCodes.Status200OK )]
        [ProducesResponseType( StatusCodes.Status400BadRequest, Type = typeof( ErrorResponse ) )]
        [ProducesResponseType( StatusCodes.Status404NotFound, Type = typeof( ErrorResponse ) )]
        public async Task<IResult> AddRoleToUser( Guid userId, int roleId ) {
            await _auth.AddRoleToUserAsync( userId, roleId );
            return Results.Ok();
        }
        /// <summary>
        /// Used to remove the role from the user.
        /// </summary>
        /// <param name="userId">Id of the user</param>
        /// <param name="roleId"> Id of tre role</param>
        /// <returns>Nothing</returns>
        /// <response code="200">If the role successfully removed from the user </response>
        /// <response code="404">If user doesn't exist </response>
        [HttpPost( "[action]" )]
        [ProducesResponseType( StatusCodes.Status200OK, Type = typeof( ErrorResponse ) )]
        [ProducesResponseType( StatusCodes.Status404NotFound, Type = typeof( ErrorResponse ) )]
        public async Task<IResult> RemoveRoleFromUser( Guid userId, int roleId ) {
            await _auth.RemoveRoleFromUserAsync( userId, roleId );
            return Results.Ok();
        }
        /// <summary>
        /// Used to obtain array of roles of the user.
        /// </summary>
        /// <param name="userId">Id of the user</param>

        /// <returns> Array of roles of the user</returns>
        /// <response code="200">If the user succesfully finded </response>
        /// <response code="404">If user doesn't exist </response>
        [HttpGet( "[action]" )]
        [ProducesResponseType( StatusCodes.Status200OK )]
        [ProducesResponseType( StatusCodes.Status404NotFound, Type = typeof( ErrorResponse ) )]
        public async Task<IResult> GetRoles( Guid userId ) {
            var roles = await _auth.GetRoles( userId );
            return Results.Ok( roles );
        }
        /// <summary>
        /// Used to obtain array of users.
        /// </summary>
        /// <returns> Array of users or empty array</returns>
        /// <response code="200">If the user succesfully finded </response>

        [HttpGet( "[action]" )]
        [ProducesResponseType( StatusCodes.Status200OK )]
        public async Task<IResult> GetUsers() {
            var users = await _auth.GetUsers();
            return Results.Ok( users );
        }
        /// <summary>
        /// Used to obtain user with his roles.
        /// </summary>
        /// <returns> User with roles</returns>
        /// <response code="200">If the user succesfully finded </response>
        /// <response code="404">If user doesn't exist </response>
        [HttpGet( "[action]" )]
        [ProducesResponseType( StatusCodes.Status200OK )]
        public async Task<IResult> GetUserWithRoles( Guid userId ) {
            var user = await _auth.GetUserWithRoles( userId );
            return Results.Ok( user );
        }
        [HttpGet( "[action]" )]
        public async Task<IResult> ThrowException() {
            throw new NotImplementedException();
        }

    }
}
