using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Profiles.Application.Common.Behavior;
using Profiles.Application.Consumers;

namespace Profiles.Application {
    public static class DependencyInjection {
        public static IServiceCollection AddApplicationLayer( this IServiceCollection services, IConfiguration config ) {
            MapsterConfiguration.Configure();

            services.AddMediatR( cfg => {
                cfg.RegisterServicesFromAssembly( typeof( DependencyInjection ).Assembly );
                cfg.AddOpenBehavior( typeof( LoggingPipelineBehavior<,> ) );
                cfg.AddOpenBehavior( typeof( AuthorizationBehaviour<,> ) );
            } );
            services.AddMassTransit( x => {
                x.SetKebabCaseEndpointNameFormatter();

                x.AddConsumer<SpecializationUpdatedConsumer>();
                x.AddConsumer<SpecializationDeletedConsumer>();
                x.AddConsumer<SpecializationCreatedConsumer>();

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
