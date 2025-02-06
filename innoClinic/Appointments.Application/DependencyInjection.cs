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

                x.AddConsumer<ServiceCreatedConsumer>();
                x.AddConsumer<ServiceDeletedConsumer>();
                x.AddConsumer<ServiceUpdatedConsumer>();
                x.AddConsumer<OfficeCreatedConsumer>();
                x.AddConsumer<OfficeDeletedConsumer>();
                x.AddConsumer<OfficeUpdatedConsumer>();
                x.AddConsumer<DoctorCreatedConsumer>();
                x.AddConsumer<DoctorDeletedConsumer>();
                x.AddConsumer<DoctorUpdatedConsumer>();
                x.AddConsumer<PatientCreatedConsumer>();
                x.AddConsumer<PatientDeletedConsumer>();
                x.AddConsumer<PatientUpdatedConsumer>();

                x.UsingRabbitMq( ( context, cfg ) => {

                    cfg.ReceiveEndpoint( "q_service-created_appointment", e => e.ConfigureConsumer<ServiceCreatedConsumer>( context ) );
                    cfg.ReceiveEndpoint( "q_service-deleted_appointment", e => e.ConfigureConsumer<ServiceDeletedConsumer>( context ) );
                    cfg.ReceiveEndpoint( "q_service-updated_appointment", e => e.ConfigureConsumer<ServiceUpdatedConsumer>( context ) );
                    cfg.ReceiveEndpoint( "q_office-created_appointment", e => e.ConfigureConsumer<OfficeCreatedConsumer>( context ) );
                    cfg.ReceiveEndpoint( "q_office-deleted_appointment", e => e.ConfigureConsumer<OfficeDeletedConsumer>( context ) );
                    cfg.ReceiveEndpoint( "q_office-updated_appointment", e => e.ConfigureConsumer<OfficeUpdatedConsumer>( context ) );
                    cfg.ReceiveEndpoint( "q_doctor-created_appointment", e => e.ConfigureConsumer<DoctorCreatedConsumer>( context ) );
                    cfg.ReceiveEndpoint( "q_doctor-deleted_appointment", e => e.ConfigureConsumer<DoctorDeletedConsumer>( context ) );
                    cfg.ReceiveEndpoint( "q_doctor-updated_appointment", e => e.ConfigureConsumer<DoctorUpdatedConsumer>( context ) );
                    cfg.ReceiveEndpoint( "q_patient-created_appointment", e => e.ConfigureConsumer<PatientCreatedConsumer>( context ) );
                    cfg.ReceiveEndpoint( "q_patient-deleted_appointment", e => e.ConfigureConsumer<PatientDeletedConsumer>( context ) );
                    cfg.ReceiveEndpoint( "q_patient-updated_appointment", e => e.ConfigureConsumer<PatientUpdatedConsumer>( context ) );

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
