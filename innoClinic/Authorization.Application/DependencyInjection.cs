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
