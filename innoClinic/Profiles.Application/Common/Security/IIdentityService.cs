namespace Profiles.Application.Common.Security {
    public interface IIdentityService {
        bool IsAuthenticated();
        bool IsInRole( string role );
    }
}
