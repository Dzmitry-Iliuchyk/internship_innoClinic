using Authorization.Application.Abstractions.Repositories;
using Authorrization.Api.Models;
using Microsoft.EntityFrameworkCore;
using Shared.Abstractions.Entities;
using Shared.Abstractions.Repository;

namespace Authorization.DataAccess.Repositories {
    public sealed class UserRepository: BaseRepository<User>, IUserRepository {

        public UserRepository( AuthDbContext context ) : base( context ) {
        }

        public async Task<bool> AnyAsync( string email ) {
            return await entities.AnyAsync( u => u.Email == email );
        }

        public async Task<bool> AnyAsync( Guid userId ) {
            return await entities.AnyAsync( u => u.Id == userId );
        }

        public async Task<User?> GetAsync( Guid userId ) {
            return await entities
                .AsNoTracking()
                .FirstOrDefaultAsync( u => u.Id == userId );
        }
        public async Task<User?> GetAsync( string email ) {
            return await entities
                .AsNoTracking()
                .FirstOrDefaultAsync( u => u.Email == email );
        }

        public async Task<IEnumerable<Role>?> GetRolesAsync( Guid userId ) {
            return await entities
                .AsNoTracking()
                .Include( u => u.Roles )
                .Where(u=>u.Id == userId)
                .Select(u=>u.Roles)
                .FirstOrDefaultAsync();
        }

        public async Task<User?> GetUserWithRolesAsync( string email ) {
            return await entities
                .AsNoTracking()
                .Include( u => u.Roles )
                .FirstOrDefaultAsync( x => x.Email == email );
        }

        public async Task<User?> GetUserWithRolesAsync( Guid userId ) {
            return await entities
                .AsNoTracking()
                .Include( u => u.Roles )
                .FirstOrDefaultAsync( x => x.Id == userId );
        }
    }
}
