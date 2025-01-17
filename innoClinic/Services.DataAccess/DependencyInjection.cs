using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services.Application.Abstractions.Repositories;
using Services.DataAccess.Repositories;


namespace Services.DataAccess {
    public static class DependencyInjection {
        public static IServiceCollection AddDataAccess(this IServiceCollection services, IConfiguration config) {
            services.AddDbContext<ServiceContext>( p => {
                p.UseSqlServer(config.GetConnectionString( "Services" ) );
                p.UseSeeding((c,_)=> Seeder.Seed(c));
                } );
            services.AddScoped<IServiceRepository, ServiceRepository>();
            services.AddScoped<IServiceCategoryRepository, ServiceCategoryRepository>();
            services.AddScoped<ISpecializationRepository, SpecializationRepository>();
            return services;
        }
    }
}
