using Profiles.Application.Common.Security;

namespace Profiles.Api {
    public class IdentityService: IIdentityService {
        private readonly IHttpContextAccessor _contextAccessor;
        public IdentityService( IHttpContextAccessor contextAccessor ) {
            this._contextAccessor = contextAccessor;
        }

        public bool IsAuthenticated() {
            return _contextAccessor.HttpContext?.User?.Identity?.IsAuthenticated??false;
        }

        public bool IsInRole( string role ) {
            return _contextAccessor.HttpContext?.User?.IsInRole(role)??false;
        }
        
    }
}