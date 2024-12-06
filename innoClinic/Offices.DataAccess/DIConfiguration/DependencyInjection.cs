using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using Offices.Application.Interfaces.Repositories;
using Offices.Application.Interfaces.Services;
using Offices.DataAccess.Mapper;
using Offices.DataAccess.Repositories;
using Offices.DataAccess.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Offices.DataAccess.DIConfiguration {
    public static class DependencyInjection {
        public static IServiceCollection AddOfficesDataAccess( this IServiceCollection services, IConfiguration configuration ) {

            var mongoDbSettings = configuration.GetRequiredSection( "OfficesDatabaseSettings" );
            services.AddSingleton<IMongoClient>( new MongoClient( mongoDbSettings[ nameof( OfficesDatabaseSettings.ConnectionString ) ] ) );
            services.AddSingleton( serviceProvider => {
                var client = serviceProvider.GetRequiredService<IMongoClient>();
                return client.GetDatabase( mongoDbSettings[ nameof( OfficesDatabaseSettings.DatabaseName ) ] );
            } );
            services.AddAutoMapper(typeof(OfficeProfile));
            services.AddScoped<IOfficeRepository, OfficeRepository>();
            services.AddScoped<IIdGenerator, MongoIdGenerator>();
            return services;
        }
    }
}
