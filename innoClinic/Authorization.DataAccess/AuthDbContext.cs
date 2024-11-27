using Authorization.DataAccess.DbConfigurations;
using Authorization.Domain.Models.Enums;
using Authorrization.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Authorization.DataAccess {
    public class AuthDbContext: DbContext {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public AuthDbContext( DbContextOptions options ) : base( options ) {

        }
        protected override void OnModelCreating( ModelBuilder modelBuilder ) {
            modelBuilder.ApplyConfigurationsFromAssembly( typeof( RoleConfiguration ).Assembly );
        }

        public void SeedUsers() {
            List<User> users = new List<User>();
            var hasher = new PasswordHasher<User>();
            for (int i = 0; i < 5; i++) {
                var guid = Guid.NewGuid();
                var user = new User {
                    Id = guid,
                    Email = $"TestUser{i}@test.com",
                    PasswordHash = hasher.HashPassword( null, guid.ToString() ),
                };
                users.Add( user );
            }
            Users.AddRange( users );
            SaveChanges();
            var roles = Enum
               .GetValues<Roles>()
               .Select( x => new Role {
                   Id = (int)x,
                   Name = x.ToString(),
               } );
            Roles.AddRange( roles );
            SaveChanges();
            var userRoles = new List<UserRole>();
            foreach (var user in users) {
                userRoles.Add( new() { UserId = user.Id, RoleId = 2 } );
            }
            userRoles.Add( new() { UserId = users[ 0 ].Id, RoleId = 1 } );
            userRoles.Add( new() { UserId = users[ 3 ].Id, RoleId = 3 } );

            UserRoles.AddRange( userRoles );
            SaveChanges();
        }
    }
}
