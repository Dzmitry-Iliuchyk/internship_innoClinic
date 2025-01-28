using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Profiles.Application.Interfaces.Repositories;
using Profiles.DataAccess.Repositories;
using Profiles.DataAccess.RepositoriesDapper;

namespace Profiles.DataAccess {
    public static class DependencyInjection {
        public static IServiceCollection AddDataAccess( this IServiceCollection services, IConfiguration configuration ) {
            var connectionString = configuration.GetConnectionString( "Profiles" );
            services.AddSqlServer<ProfilesDbContext>(connectionString);
            services.AddSingleton<DapperProfileContext>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IAccountReadRepository, AccountReadRepository>();
            services.AddScoped<IDoctorCommandRepository, DoctorCommandRepository>();
            services.AddScoped<IPatientCommandRepository, PatientCommandRepository>();
            services.AddScoped<IReceptionistCommandRepository, ReceptionistCommandRepository>();
            services.AddScoped<IDoctorReadRepository, DoctorReadRepository>();
            services.AddScoped<IPatientReadRepository, PatientReadRepository>();
            services.AddScoped<IReceptionistReadRepository, ReceptionistReadRepository>();
            return services;
        }
    }
}
