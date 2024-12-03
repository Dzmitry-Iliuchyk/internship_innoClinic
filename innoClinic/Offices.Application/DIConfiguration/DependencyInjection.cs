using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Offices.Application.AutoMapper;
using Offices.Application.FluentValidator;
using Offices.Application.Implementations.Services;
using Offices.Application.Interfaces.Services;

namespace Offices.Application.DIConfiguration {
    public static class DependencyInjection {
        public static IServiceCollection AddOfficesApplication(this IServiceCollection services) {
            services.AddValidatorsFromAssemblyContaining<OfficeValidator>();
            services.AddAutoMapper(typeof(OfficeProfile));
            services.AddScoped<IOfficeService, OfficeService>();
            services.Decorate<IOfficeService, LoggingOfficeService>();
            
            return services;
        }
    }
}
