using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Notifications.Application.Consumers;
using Notifications.Application.Interfaces;
using Notifications.Application.Services;
using Shared.Events.Contracts;
using static MassTransit.MessageHeaders;


namespace Notifications.Application {
    public static class DependencyInjection {
        public static IServiceCollection AddApplication( this IServiceCollection services, IConfiguration config ) {
            var emailConfiguration = config.GetRequiredSection( nameof( EmailConfiguration ) );
            services.Configure<EmailConfiguration>( x => {
                x.From = emailConfiguration[ "From" ] ?? throw new ArgumentNullException( "From" );
                x.SmtpServer = emailConfiguration[ "SmtpServer" ] ?? throw new ArgumentNullException( "SmtpServer" );
                x.UserName = emailConfiguration[ "UserName" ] ?? throw new ArgumentNullException( "UserName" );
                x.Password = emailConfiguration[ "Password" ] ?? throw new ArgumentNullException( "Password" );
                x.Port = int.Parse( emailConfiguration[ "Port" ] ?? throw new ArgumentNullException( "Port" ) );
            } );
            services.AddSingleton<IEmailSender, EmailSender>();
            services.AddMassTransit( x => {
                x.SetKebabCaseEndpointNameFormatter();

                x.AddConsumer<SendEmailConsumer>();

                x.UsingRabbitMq( ( context, cfg ) => {
                    cfg.Host( config[ "rabbitMq:host" ] ?? throw new ArgumentNullException( "rabbitMq:host" ),
                        "/", h => {
                        h.Username( config[ "rabbitMq:user" ] ?? throw new ArgumentNullException( "rabbitMq:user" ) );
                        h.Password( config[ "rabbitMq:password" ]??throw new ArgumentNullException( "rabbitMq:password" ) );
                    } );

                    cfg.ConfigureEndpoints( context );
                } );
                //x.AddRequestClient<SendEmailRequest>();

            } );
    
            return services;
        }
    }

}
