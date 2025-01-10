using Appointments.Application.Implementations;
using Appointments.Application.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Appointments.Application {
    public static class DependencyInjection{
        public static IServiceCollection AddApplicationLayer( this IServiceCollection services ) {
            services.AddScoped<IAppointmentService,AppointmentsService>();
            services.AddScoped<IResultService,ResultService>();

            return services;
        }
    }
}
