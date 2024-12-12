using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Profiles.Application.Common.Behavior;

namespace Profiles.Application {
    public static class DependencyInjection {
        public static IServiceCollection AddApplicationLayer( this IServiceCollection services) {
            MapsterConfiguration.Configure();

            services.AddMediatR( cfg => {
                cfg.RegisterServicesFromAssembly( typeof( DependencyInjection ).Assembly );
                cfg.AddBehavior( typeof( IPipelineBehavior<,> ), typeof( LoggingPipelineBehavior<,> ) );
                cfg.AddBehavior( typeof( IPipelineBehavior<,> ), typeof( AuthorizationBehaviour<,> ) );
            } );

            return services;
        }

    }
}
