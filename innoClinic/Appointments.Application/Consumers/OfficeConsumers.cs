using Appointments.Application.Interfaces.Repositories;
using MassTransit;
using Shared.Events.Contracts.OfficesMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appointments.Application.Consumers {
    public class OfficeCreatedConsumer: IConsumer<OfficeCreated> {
        private readonly IOfficeRepository _offices;
        public OfficeCreatedConsumer( IOfficeRepository offices ) {
            this._offices = offices;
        }
        public async Task Consume( ConsumeContext<OfficeCreated> context ) {

            await _offices.CreateAsync(new Domain.Office {
                Id = context.Message.Id,
                RegistryPhoneNumber = context.Message.RegistryPhoneNumber,
                OfficeAddress = context.Message.Address,
            } );

        }
    }
    public class OfficeUpdatedConsumer: IConsumer<OfficeUpdated> {
        private readonly IOfficeRepository _offices;
        public OfficeUpdatedConsumer( IOfficeRepository offices ) {
            this._offices = offices;
        }
        public async Task Consume( ConsumeContext<OfficeUpdated> context ) {

            await _offices.UpdateAsync(new Domain.Office {
                Id = context.Message.Id,
                RegistryPhoneNumber = context.Message.RegistryPhoneNumber,
                OfficeAddress = context.Message.Address,
            } );

        }
    }
    public class OfficeDeletedConsumer: IConsumer<OfficeDeleted> {
        private readonly IOfficeRepository _offices;
        public OfficeDeletedConsumer( IOfficeRepository offices ) {
            this._offices = offices;
        }
        public async Task Consume( ConsumeContext<OfficeDeleted> context ) {
            var office = await _offices.GetAsync(context.Message.Id);
            await _offices.DeleteAsync( office );

        }
    }
}
