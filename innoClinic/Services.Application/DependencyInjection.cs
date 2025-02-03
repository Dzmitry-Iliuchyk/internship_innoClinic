using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services.Application.Abstractions.Repositories;
using Services.Application.Abstractions.Services;
using Services.Application.Implementations.Services;

namespace Services.Application {
    public static class DependencyInjection {
        public static IServiceCollection AddApplication( this IServiceCollection services, IConfiguration config ) {
            MapsterConfiguration.Configure();

            services.AddScoped<IServiceService, ServiceService>();
            services.AddScoped<IServiceCategoryService, ServiceCategoryService>();
            services.AddScoped<ISpecializationService, SpecializationService>();
            services.AddMassTransit( x => {
                x.SetKebabCaseEndpointNameFormatter();

                //x.AddConsumer<>();
                x.UsingRabbitMq( ( context, cfg ) => {
                    cfg.Host( config[ "rabbitMq:host" ] ?? throw new ArgumentNullException( "rabbitMq:host" ),
                        "/", h => {
                            h.Username( config[ "rabbitMq:user" ] ?? throw new ArgumentNullException( "rabbitMq:user" ) );
                            h.Password( config[ "rabbitMq:password" ] ?? throw new ArgumentNullException( "rabbitMq:password" ) );
                        } );

                    cfg.ConfigureEndpoints( context );
                } );

            } );
            return services;
        }
    }
}
