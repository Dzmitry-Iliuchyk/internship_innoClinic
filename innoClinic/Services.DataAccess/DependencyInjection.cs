using LinqToDB;
using LinqToDB.AspNet;
using LinqToDB.AspNet.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services.Application.Abstractions.Repositories;
using Services.DataAccess.Repositories;


namespace Services.DataAccess {
    public static class DependencyInjection {
        public static IServiceCollection AddDataAccess(this IServiceCollection services, IConfiguration config) {
            services.AddLinqToDBContext<ServicesDataConnection>( ( provider, options )
            => options
                .UseSqlServer( config.GetConnectionString( "Default" )! )
                .UseMappingSchema(new ServicesMappingSchema())
                .UseDefaultLogging( provider ) );
            services.AddScoped<IServiceRepository, ServiceRepository>();
            return services;
        }
    }
}
