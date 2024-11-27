using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Authorization.DataAccess {
    public static class DependencyInjection {
        public static IServiceCollection ConfigureAuthDataAccess(this IServiceCollection services, string connectionString ) {
            return services.AddDbContext<AuthDbContext>( opt => opt.UseSqlServer( connectionString ) );
        }
    }
}
