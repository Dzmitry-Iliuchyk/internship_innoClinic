using AutoMapper;
using Consul;
using FluentValidation;
using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Mongo2Go;
using MongoDB.Driver;
using Moq;
using Offices.Application.Implementations.Services;
using Offices.Application.Interfaces.Repositories;
using Offices.Application.Interfaces.Services;
using Offices.DataAccess.DIConfiguration;
using Offices.DataAccess.Repositories;
using Offices.Domain.Models;
using Shared.ServiceDiscovery;
namespace Offices.IntegrationTests {

    public class CustomWebApplicationFactory: WebApplicationFactory<Program> {
        private readonly MongoDbRunner _mongoRunner;
        private readonly IMongoDatabase _testDatabase;

        public CustomWebApplicationFactory() {
            _mongoRunner = MongoDbRunner.Start();
            var client = new MongoClient( _mongoRunner.ConnectionString );
            _testDatabase = client.GetDatabase( "TestDb" );
        }

        protected override void ConfigureWebHost( IWebHostBuilder builder ) {
            builder.ConfigureServices( services => {
                // Удаляем регистрацию IOfficeRepository
                var repoDescriptor = services.SingleOrDefault( d => d.ServiceType == typeof( IOfficeRepository ) );
                if (repoDescriptor != null) {
                    services.Remove( repoDescriptor );
                }
                var descriptor = services.SingleOrDefault( d => d.ServiceType == typeof( IPublishEndpoint ) );
                if (descriptor != null) {
                    services.Remove( descriptor );
                }
                // Удаляем реальную регистрацию MassTransit
                var masstransitDescriptor = services.SingleOrDefault( d => d.ServiceType == typeof( IBusControl ) );
                if (masstransitDescriptor != null) {
                    services.Remove( masstransitDescriptor );
                }

                // Регистрируем MassTransit с InMemory транспортом
                services.AddMassTransitTestHarness( cfg =>
                {
                    cfg.SetKebabCaseEndpointNameFormatter();

                    // Если у вас есть консумеры, добавьте их
                    // cfg.AddConsumer<YourConsumer>();

                    cfg.UsingInMemory( ( context, cfg ) =>
                    {
                        cfg.ConfigureEndpoints( context );
                    } );
                } );
 
                // Удаляем регистрацию IOfficeService
                var serviceDescriptor = services.FirstOrDefault( d => d.ServiceType == typeof( IOfficeService ) );
                if (serviceDescriptor != null) {
                    services.RemoveAll<IOfficeService>( );
                }
                

                // Создаем поддельные настройки базы данных
                var testSettings = Options.Create( new OfficesDatabaseSettings {
                    OfficesCollectionName = "Offices",
                    DatabaseName = "test",
                } );

                // Добавляем новый IOfficeRepository с тестовой базой MongoDB
                services.AddScoped<IOfficeRepository>( sp => {
                    var mapper = sp.GetRequiredService<IMapper>();
                    return new OfficeRepository( _testDatabase, testSettings, mapper );
                } );

                // Добавляем новый IOfficeService, использующий фейковый репозиторий
                services.AddScoped<IOfficeService>( sp => {
                    var mapper = sp.GetRequiredService<IMapper>();
                    var validator = sp.GetRequiredService<IValidator<Office>>();
                    var repo = sp.GetRequiredService<IOfficeRepository>();
                    var gen = sp.GetRequiredService<IIdGenerator>();
                    var publisher = sp.GetRequiredService<IPublishEndpoint>();
                    return new OfficeService( repo, validator, mapper, gen, publisher );
                } );

                // Удаляем реальный IConsulClient
                var consulDescriptor = services.SingleOrDefault( d => d.ServiceType == typeof( IConsulClient ) );
                if (consulDescriptor != null) {
                    services.Remove( consulDescriptor );
                }

                var serviceDiscoveryDescriptor = services.SingleOrDefault( d => d.ServiceType == typeof( IHostedService ) && d.ImplementationType == typeof( ServiceDiscoveryHostedService ) );
                if (serviceDiscoveryDescriptor != null) {
                    services.Remove( serviceDiscoveryDescriptor );
                }


                //    // Создаём Mock для IConsulClient
                //    var mockConsulClient = new Mock<IConsulClient>();

                //    // Настраиваем метод Agent.ServiceRegister (чтобы он ничего не делал)
                //    var mockAgentService = new Mock<IAgentEndpoint>();
                //    mockAgentService
                //        .Setup( a => a.ServiceRegister( It.IsAny<AgentServiceRegistration>(), It.IsAny<CancellationToken>() ) )
                //        .Returns( Task.FromResult(new Consul.WriteResult() {
                //            RequestTime = TimeSpan.FromMilliseconds(30),
                //            StatusCode = System.Net.HttpStatusCode.OK,
                //        } ));

                //    // Настраиваем метод Agent.ServiceDeregister (чтобы он ничего не делал)
                //    mockAgentService
                //        .Setup( a => a.ServiceDeregister( It.IsAny<string>(), It.IsAny<CancellationToken>() ) )
                //        .Returns( Task.FromResult( new Consul.WriteResult() {
                //            RequestTime = TimeSpan.FromMilliseconds( 30 ),
                //            StatusCode = System.Net.HttpStatusCode.OK,
                //        } ) );

                //    // Подменяем Agent на замоканный
                //    mockConsulClient.Setup( c => c.Agent ).Returns( mockAgentService.Object );

                //    // Регистрируем мок вместо реального IConsulClient
                //    services.AddSingleton( mockConsulClient.Object );
                } );
            }
    }

}