using Appointments.Application.Interfaces.Repositories;
using MassTransit;
using Shared.Events.Contracts.ServiceMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appointments.Application.Consumers {
    public class ServiceCreatedConsumer: IConsumer<ServiceCreated> {
        private readonly IServiceRepository _services;
        public ServiceCreatedConsumer( IServiceRepository services ) {
            this._services = services;
        }
        public async Task Consume( ConsumeContext<ServiceCreated> context ) {

            await _services.CreateAsync( new Domain.Service {
                Id = context.Message.Id,
                ServiceName = context.Message.Name,

            } );

        }
    }
    public class ServiceUpdatedConsumer: IConsumer<ServiceUpdated> {
        private readonly IServiceRepository _services;
        public ServiceUpdatedConsumer( IServiceRepository services ) {
            this._services = services;
        }
        public async Task Consume( ConsumeContext<ServiceUpdated> context ) {

            await _services.UpdateAsync( new Domain.Service {
                Id = context.Message.Id,
                ServiceName = context.Message.Name,

            } );

        }
    }
    public class ServiceDeletedConsumer: IConsumer<ServiceDeleted> {
        private readonly IServiceRepository _services;
        public ServiceDeletedConsumer( IServiceRepository services ) {
            this._services = services;
        }
        public async Task Consume( ConsumeContext<ServiceDeleted> context ) {

            await _services.DeleteAsync( new Domain.Service {
                Id = context.Message.Id

            } );

        }
    }
}
