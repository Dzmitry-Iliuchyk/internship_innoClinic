using MediatR;
using Microsoft.AspNet.Identity;
using Profiles.Application.Common.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
                    throw new UnauthorizedAccessException();
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

    public class ForbiddenAccessException: Exception {
        public ForbiddenAccessException() : base() { }
    }
}
