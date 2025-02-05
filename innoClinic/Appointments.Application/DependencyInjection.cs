using Appointments.Application.Consumers;
using Appointments.Application.Implementations;
using Appointments.Application.Interfaces.Services;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Events.Contracts;

namespace Appointments.Application {
    public static class DependencyInjection{
        public static IServiceCollection AddApplicationLayer( this IServiceCollection services, IConfiguration config ) {
            services.AddScoped<IAppointmentService,AppointmentsService>();
            services.AddScoped<IResultService,ResultService>();
            services.AddMassTransit( x => {
                x.SetKebabCaseEndpointNameFormatter();

                x.AddConsumer<DoctorCreatedConsumer>();
                x.AddConsumer<DoctorDeletedConsumer>();
                x.AddConsumer<DoctorUpdatedConsumer>();
                x.AddConsumer<PatientCreatedConsumer>();
                x.AddConsumer<PatientDeletedConsumer>();
                x.AddConsumer<PatientUpdatedConsumer>();
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
