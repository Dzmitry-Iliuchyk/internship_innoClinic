﻿using Consul;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Shared.ServiceDiscovery {
    public static class ServiceDiscoveryExtensions {
        public static void RegisterConsulServices( this IServiceCollection services, ServiceConfig serviceConfig ) {

            ArgumentNullException.ThrowIfNull(serviceConfig, nameof( serviceConfig ) );

            var consulClient = CreateConsulClient( serviceConfig );

            services.AddSingleton( serviceConfig );
            services.AddSingleton<IHostedService, ServiceDiscoveryHostedService>();
            services.AddSingleton<IConsulClient, ConsulClient>( p => consulClient );
        }

        private static ConsulClient CreateConsulClient( ServiceConfig serviceConfig ) {
            return new ConsulClient( config =>
            {
                config.Address = serviceConfig.ServiceDiscoveryAddress;
            } );
        }
    }

}
