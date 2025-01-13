using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


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
            return services;
        }
    }

}
