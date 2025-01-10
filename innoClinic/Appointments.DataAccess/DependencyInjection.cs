using Appointments.Application.Interfaces.Repositories;
using Appointments.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Appointments.DataAccess {
    public static class DependencyInjection{
        public static IServiceCollection AddDataAccess(this IServiceCollection services, IConfiguration config) {

            services.AddDbContext<AppointmentsDbContext>( options => {
                options.UseNpgsql(config.GetConnectionString( "Appointments" ) );
            } );
            services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            services.AddScoped<IResultRepository, ResultRepository>();
            return services;
        }
    }
}
