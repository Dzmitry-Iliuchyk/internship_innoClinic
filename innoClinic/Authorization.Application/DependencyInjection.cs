using Authorization.Application.Abstractions.Repositories;
using Authorization.Application.Abstractions.Services;
using Authorization.Application.Consumers;
using Authorization.Application.Implementations;
using Authorization.Application.Validations;
using Authorrization.Api.Models;
using FluentValidation;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Cryptography;

namespace Authorization.Application {
    public static class DependencyInjection {
        public static IServiceCollection ConfigureAuthApplicationDependncies( this IServiceCollection services, IConfiguration config) {
            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddValidatorsFromAssemblyContaining<UserValidator>();
            services.AddMassTransit( x => {
                x.SetKebabCaseEndpointNameFormatter();

                x.AddConsumer<PatientCreatedConsumer>();
                x.AddConsumer<PatientDeletedConsumer>();
                x.AddConsumer<PatientUpdatedConsumer>();
                x.AddConsumer<DoctorDeletedConsumer>();
                x.AddConsumer<DoctorUpdatedConsumer>();
                x.AddConsumer<DoctorCreatedConsumer>();
                x.AddConsumer<ReceptionistCreatedConsumer>();
                x.AddConsumer<ReceptionistDeletedConsumer>();
                x.AddConsumer<ReceptionistUpdatedConsumer>();

                x.UsingRabbitMq( ( context, cfg ) => {

                    cfg.ReceiveEndpoint( "q_doctor-created-auth", e => e.ConfigureConsumer<DoctorCreatedConsumer>( context ) );
                    cfg.ReceiveEndpoint( "q_doctor-deleted-auth", e => e.ConfigureConsumer<DoctorDeletedConsumer>( context ) );
                    cfg.ReceiveEndpoint( "q_doctor-updated-auth", e => e.ConfigureConsumer<DoctorUpdatedConsumer>( context ) );
                    cfg.ReceiveEndpoint( "q_patient-created-auth", e => e.ConfigureConsumer<PatientCreatedConsumer>( context ) );
                    cfg.ReceiveEndpoint( "q_patient-deleted-auth", e => e.ConfigureConsumer<PatientDeletedConsumer>( context ) );
                    cfg.ReceiveEndpoint( "q_patient-updated-auth", e => e.ConfigureConsumer<PatientUpdatedConsumer>( context ) );
                    cfg.ReceiveEndpoint( "q_receptionist-created-auth", e => e.ConfigureConsumer<ReceptionistCreatedConsumer>( context ) );
                    cfg.ReceiveEndpoint( "q_receptionist-deleted-auth", e => e.ConfigureConsumer<ReceptionistDeletedConsumer>( context ) );
                    cfg.ReceiveEndpoint( "q_receptionist-updated-auth", e => e.ConfigureConsumer<ReceptionistUpdatedConsumer>( context ) );

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
