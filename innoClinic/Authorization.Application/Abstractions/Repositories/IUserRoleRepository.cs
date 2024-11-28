using Authorrization.Api.Models;
using Shared.Abstractions.Repository;

namespace Authorization.Application.Abstractions.Repositories {
    public interface IUserRoleRepository: IRepository<UserRole> {
        Task<bool> AnyAsync( Guid userId, int roleId );
        Task<UserRole?> GetAsync( int roleId, Guid userId );
    }
}
