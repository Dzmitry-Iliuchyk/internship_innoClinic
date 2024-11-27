using Authorrization.Api.Models;

namespace Authorization.Domain.Repositories {
    public interface IRoleRepository {
        Task Update( Role updatedRole);
        Task Create( Role role );
        Task Delete( Role role );
        Task<Role> Get( int roleId );
        Task<IEnumerable<Role>> GetAll();
    }
}
