using Microsoft.Extensions.DependencyInjection;
using Services.Application.Abstractions.Repositories;
using Services.Application.Abstractions.Services;
using Services.Application.Implementations.Services;

namespace Services.Application {
    public static class DependencyInjection {
        public static IServiceCollection AddApplication( this IServiceCollection services ) {
            MapsterConfiguration.Configure();

            services.AddScoped<IServiceService, ServiceService>();
            services.AddScoped<IServiceCategoryService, ServiceCategoryService>();
            services.AddScoped<ISpecializationService, SpecializationService>();

            return services;
        }
    }
}
