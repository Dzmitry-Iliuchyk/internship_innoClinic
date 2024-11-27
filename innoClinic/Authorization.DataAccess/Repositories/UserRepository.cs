using Authorization.Domain.Repositories;
using Authorrization.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Authorization.DataAccess.Repositories {
    public class UserRepository: IUserRepository {
        private readonly AuthDbContext _authDbContext;
        private readonly DbSet<User> _users;
        public UserRepository(AuthDbContext context) {
            _authDbContext = context;
            _users = context.Set<User>();
        }

        public async Task Create( User user ) {
            if (await _users.AnyAsync(u=>u.Id== user.Id)) {
                throw new Exception();//todo
            }
            await _users.AddAsync( user );
            await _authDbContext.SaveChangesAsync();
        }

        public async Task Delete( User user ) {
            _users.Remove( user );
            await _authDbContext.SaveChangesAsync();
        }

        public async Task<User> Get( Guid userId ) {
            return await _users
                .AsNoTracking()
                .FirstAsync( x => x.Id == userId );
        }

        public async Task<IEnumerable<User>> GetAll() {
            return await _users
                .AsNoTracking()
                .ToArrayAsync();
        }

        public async Task<IEnumerable<Role>> GetRoles( Guid userId ) {
            return (await _users
                .AsNoTracking()
                .Include(u=>u.Roles)
                .FirstAsync( x => x.Id == userId ))?.Roles??[];
        }

        public async Task Update( User updatedUser ) {
            if (!_users.Any(x=>x.Id == updatedUser.Id)) {
                throw new Exception();//todo
            }
            _authDbContext.Update( updatedUser );
            await _authDbContext.SaveChangesAsync();
        }
    }
}
