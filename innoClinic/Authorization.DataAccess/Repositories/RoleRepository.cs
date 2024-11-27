using Authorization.Domain.Repositories;
using Authorrization.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Authorization.DataAccess.Repositories {
    public class RoleRepository: IRoleRepository {
        private readonly AuthDbContext _authDbContext;
        private readonly DbSet<Role> _roles;
        public RoleRepository(AuthDbContext context) {
            _authDbContext = context;
            _roles = context.Set<Role>();
        }
        public async Task Create( Role role ) {
            if (await _roles.AnyAsync(r=>r.Id == role.Id)) {
                throw new Exception();
            }
            await _roles.AddAsync(role);
            await _authDbContext.SaveChangesAsync();
        }

        public async Task Delete( Role role ) {
            _roles.Remove(role);
            await _authDbContext.SaveChangesAsync();
        }

        public async Task<Role> Get( int roleId ) {
            return await _roles
                .AsNoTracking()
                .FirstAsync( r => r.Id == roleId );
        }

        public async Task<IEnumerable<Role>> GetAll() {
            return await _roles.AsNoTracking().ToArrayAsync();
        }

        public async Task Update( Role updatedRole ) {
            if (!_roles.Any( x => x.Id == updatedRole.Id )) {
                throw new Exception();//todo
            }
            _authDbContext.Update( updatedRole );
            await _authDbContext.SaveChangesAsync();
        }
    }
}
