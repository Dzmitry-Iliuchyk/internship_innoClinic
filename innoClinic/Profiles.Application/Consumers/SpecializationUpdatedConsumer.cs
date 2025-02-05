using MassTransit;
using Profiles.Application.Common.Exceptions;
using Profiles.Application.Interfaces.Repositories;
using Shared.Events.Contracts.ServiceMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profiles.Application.Consumers {
    public class SpecializationUpdatedConsumer: IConsumer<SpecializationUpdated> {
        private readonly ISpecializationRepository _repo;

        public SpecializationUpdatedConsumer( ISpecializationRepository repo ) {
            this._repo = repo;
        }

        public async Task Consume( ConsumeContext<SpecializationUpdated> context ) {

            var entity = await _repo.GetAsync( x => x.Id == context.Message.Id );
            await _repo.UpdateAsync(new Domain.Specialization {
                Id = context.Message.Id,
                Name = context.Message.Name,    
                isActive = context.Message.IsActive,
            } );
        }
    }
}
