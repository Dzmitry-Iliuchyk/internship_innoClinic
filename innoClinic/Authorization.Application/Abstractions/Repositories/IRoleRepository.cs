using Authorrization.Api.Models;
using Shared.Abstractions.Repository;

namespace Authorization.Application.Abstractions.Repositories {
    public interface IRoleRepository: IRepository<Role> {
        Task<Role?> GetAsync( int roleId );
    }
}
