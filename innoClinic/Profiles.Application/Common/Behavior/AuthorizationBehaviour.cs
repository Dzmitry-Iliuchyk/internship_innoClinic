using MediatR;
using Profiles.Application.Common.Exceptions;
using Profiles.Application.Common.Security;
using System.Reflection;

namespace Profiles.Application.Common.Behavior {
    public class AuthorizationBehaviour<TRequest, TResponse>: IPipelineBehavior<TRequest, TResponse> where TRequest : notnull {
        private readonly IIdentityService _identityService;

        public AuthorizationBehaviour( IIdentityService identityService ) {
            _identityService = identityService;
        }

        public async Task<TResponse> Handle( TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken ) {
            var authorizeAttributes = request.GetType().GetCustomAttributes<AuthorizeAttribute>();

            if (authorizeAttributes.Any()) {
                // Must be authenticated user
                if (!_identityService.IsAuthenticated()) {
                    throw new UnauthorizedAccessException("Current user does not authorized");
                }

                // Role-based authorization
                var authorizeAttributesWithRoles = authorizeAttributes.Where( a => !string.IsNullOrWhiteSpace( a.Roles ) );

                if (authorizeAttributesWithRoles.Any()) {
                    var authorized = false;

                    foreach (var roles in authorizeAttributesWithRoles.Select( a => a.Roles.Split( ',' ) )) {
                        foreach (var role in roles) {
                            var isInRole = _identityService.IsInRole(role.Trim() );
                            if (isInRole) {
                                authorized = true;
                                break;
                            }
                        }
                    }

                    // Must be a member of at least one role in roles
                    if (!authorized) {
                        throw new ForbiddenAccessException();
                    }
                }
            }

            //User is authorized / authorization not required
            return await next();
        }
    }
}
