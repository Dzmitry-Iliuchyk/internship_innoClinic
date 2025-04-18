﻿using Microsoft.Extensions.Configuration;

namespace Shared.ServiceDiscovery {
    public static class ServiceConfigExtensions {
        public static ServiceConfig GetServiceConfig( this IConfiguration configuration ) {
            ArgumentNullException.ThrowIfNull( configuration, nameof( configuration ) );

            var serviceConfig = new ServiceConfig {
                ServiceDiscoveryAddress = configuration.GetValue<Uri>( "ServiceConfig:serviceDiscoveryAddress" )!,
                ServiceAddress = configuration.GetValue<Uri>( "ServiceConfig:serviceAddress" )!,
                ServiceName = configuration.GetValue<string>( "ServiceConfig:serviceName" )!,
                ServiceId = configuration.GetValue<string>( "ServiceConfig:serviceId" )!
            };

            return serviceConfig;
        }
    }

}
