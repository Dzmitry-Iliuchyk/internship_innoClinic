using Documents.Application;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Documents.DataAccess {
    public static class DependencyInjection {
        public static IServiceCollection AddDataAccess(this IServiceCollection services, IConfiguration config) {
            services.AddScoped<IBlobStorage, AzureBlobStorage>();
            services.AddAzureClients( clientBuilder => {
                var conn = Environment.GetEnvironmentVariable( "AZURE_STORAGE_CONNECTION_STRING" );
                clientBuilder.AddBlobServiceClient( conn );
            } );
            return services;
        }
    }
}
