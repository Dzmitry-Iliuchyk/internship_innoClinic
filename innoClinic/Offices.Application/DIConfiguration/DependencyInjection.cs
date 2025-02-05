using FluentValidation;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Offices.Application.AutoMapper;
using Offices.Application.FluentValidator;
using Offices.Application.Implementations.Services;
using Offices.Application.Interfaces.Services;

namespace Offices.Application.DIConfiguration {
    public static class DependencyInjection {
        public static IServiceCollection AddOfficesApplication(this IServiceCollection services, IConfiguration config) {
            services.AddValidatorsFromAssemblyContaining<OfficeValidator>();
            services.AddAutoMapper(typeof(OfficeProfile));
            services.AddScoped<IOfficeService, OfficeService>();
            services.Decorate<IOfficeService, LoggingOfficeService>();
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
