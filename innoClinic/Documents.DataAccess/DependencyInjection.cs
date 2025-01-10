using Documents.Application;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Documents.DataAccess {
    public static class DependencyInjection {
        public static IServiceCollection AddDataAccess(this IServiceCollection services, IConfiguration config) {
            services.AddScoped<IBlobStorage, AzureBlobStorage>();
            services.AddAzureClients( clientBuilder => {
                clientBuilder.AddBlobServiceClient( config[ "StorageConnection:blobServiceUri" ]! );
            } );
            return services;
        }
    }
}
