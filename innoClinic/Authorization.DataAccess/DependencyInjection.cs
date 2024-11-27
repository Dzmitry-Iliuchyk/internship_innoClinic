using Authorization.DataAccess.Repositories;
using Authorization.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Authorization.DataAccess {
    public static class DependencyInjection {
        public static IServiceCollection ConfigureAuthDataAccess(this IServiceCollection services, string connectionString ) {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddDbContext<AuthDbContext>( opt => opt.UseSqlServer( connectionString ) );
            return services;
        }
    }
}
