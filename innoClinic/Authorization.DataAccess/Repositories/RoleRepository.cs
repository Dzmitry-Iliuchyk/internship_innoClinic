
using Authorization.Application.Abstractions.Repositories;
using Authorrization.Api.Models;
using Microsoft.EntityFrameworkCore;
using Shared.Abstractions.Repository;

namespace Authorization.DataAccess.Repositories {
    public sealed class RoleRepository: BaseRepository<Role>, IRoleRepository {

        public RoleRepository( AuthDbContext context ) : base( context ) {

        }

        public async Task<Role?> GetAsync( int roleId ) {
            return await entities.AsNoTracking().FirstOrDefaultAsync( u => u.Id == roleId );
        }

    }
}
