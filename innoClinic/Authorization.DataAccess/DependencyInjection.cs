using Authorization.Application.Abstractions.Repositories;
using Authorization.DataAccess.Repositories;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Authorization.DataAccess {
    public static class DependencyInjection {
        public static IServiceCollection ConfigureAuthDataAccess(this IServiceCollection services, string connectionString ) {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IUserRoleRepository, UserRoleRepository>();
            services.AddDbContext<AuthDbContext>( opt => opt.UseSqlServer( connectionString ) );
            return services;
        }
    }
}
