using Authorization.Application.Abstractions.Repositories;
using Authorrization.Api.Models;
using Microsoft.EntityFrameworkCore;
using Shared.Abstractions.Repository;

namespace Authorization.DataAccess.Repositories {
    public sealed class UserRoleRepository: BaseRepository<UserRole>, IUserRoleRepository {
        public UserRoleRepository( AuthDbContext context ) : base( context ) {
        }
        public async Task<UserRole?> GetAsync( int roleId, Guid userId ) {
            return await entities.AsNoTracking().FirstOrDefaultAsync( ur => ur.RoleId == roleId && ur.UserId == userId );
        }

        public async Task<bool> AnyAsync( Guid userId, int roleId ) {
            return await entities.AnyAsync( u => u.UserId == userId && u.RoleId == roleId );
        }

    }
}
