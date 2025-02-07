using MassTransit;
using Profiles.Application.Interfaces.Repositories;
using Shared.Events.Contracts.ServiceMessages;

namespace Profiles.Application.Consumers {
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
}
