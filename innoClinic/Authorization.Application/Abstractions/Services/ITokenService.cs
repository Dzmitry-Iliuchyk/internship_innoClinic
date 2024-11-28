using Authorrization.Api.Models;

namespace Authorization.Application.Abstractions.Services {
    public interface ITokenService {
        string GenerateToken( User user );
    }
}
