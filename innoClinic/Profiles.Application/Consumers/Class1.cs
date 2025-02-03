using MassTransit;
using Profiles.Application.Common.Exceptions;
using Profiles.Application.Interfaces.Repositories;
using Shared.Events.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profiles.Application.Consumers {
    public class SpecializationCreatedConsumer: IConsumer<SpecializationCreated> {
        private readonly ISpecializationRepository _repo;

        public SpecializationCreatedConsumer( ISpecializationRepository repo ) {
            this._repo = repo;
        }

        public async Task Consume( ConsumeContext<SpecializationCreated> context ) {

            await _repo.CreateAsync(new Domain.Specialization {
                Id = context.Message.Id,
                Name = context.Message.Name,
                isActive = context.Message.IsActive,
            } );
        }
    }
    public class SpecializationDeletedConsumer: IConsumer<SpecializationDeleted> {
        private readonly ISpecializationRepository _repo;

        public SpecializationDeletedConsumer( ISpecializationRepository repo ) {
            this._repo = repo;
        }

        public async Task Consume( ConsumeContext<SpecializationDeleted> context ) {

            var entity = await _repo.GetAsync(x=>x.Id == context.Message.Id);
            await _repo.DeleteAsync(entity);
        }
    }
    public class SpecializationUpdatedConsumer: IConsumer<SpecializationUpdated> {
        private readonly ISpecializationRepository _repo;

        public SpecializationUpdatedConsumer( ISpecializationRepository repo ) {
            this._repo = repo;
        }

        public async Task Consume( ConsumeContext<SpecializationUpdated> context ) {

            var entity = await _repo.GetAsync(x=>x.Id == context.Message.Id);
            if (entity==null) {
                throw new SpecializationNotFoundException(context.Message.Name);
            }
            await _repo.UpdateAsync(new Domain.Specialization {
                Id = context.Message.Id,
                Name = context.Message.Name,    
                isActive = context.Message.IsActive,
            } );
        }
    }
}
