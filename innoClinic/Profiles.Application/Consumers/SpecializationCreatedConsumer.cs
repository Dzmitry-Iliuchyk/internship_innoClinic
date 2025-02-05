using MassTransit;
using Profiles.Application.Interfaces.Repositories;
using Shared.Events.Contracts;

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
}
