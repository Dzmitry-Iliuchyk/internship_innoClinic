using Mapster;
using MassTransit;
using Services.Application.Abstractions.Repositories;
using Services.Application.Abstractions.Services;
using Services.Domain;
using Shared.Events.Contracts.ServiceMessages;

namespace Services.Application.Implementations.Services {
    public class SpecializationService: ISpecializationService {
        private readonly ISpecializationRepository _specializationRepository;
        private readonly IPublishEndpoint _publisher;

        public SpecializationService( ISpecializationRepository specializationRepository ) {
            this._specializationRepository = specializationRepository;

        }

        public async Task<int> CreateAsync( CreateSpecializationDto specialization ) {
            var spec = specialization.Adapt<Specialization>();
            var id = await _specializationRepository.CreateAsync( spec );

            await _publisher.Publish(
                new SpecializationCreated() {
                    Id = id,
                    IsActive = specialization.IsActive,
                    Name = specialization.Name,
                } );
            return id;
        }

        public async Task DeleteAsync( int specializationId ) {
            var spec = await _specializationRepository.GetLightAsync( x => x.Id == specializationId );
            if (spec == null) {
                throw new Exception();
            }
            await _specializationRepository.DeleteAsync( spec );

            await _publisher.Publish(
                new SpecializationDeleted() {
                    Id = specializationId
                } );
        }

        public async Task<IList<string>> GetAllAsync() {
            var specs = await _specializationRepository.GetAllAsync();
            return specs.Select( x => x.Name ).ToList();
        }

        public async Task<SpecializationDto?> GetAsync( int id ) {
            return ( await _specializationRepository.GetAsync( x => x.Id == id ) ).Adapt<SpecializationDto>();
        }

        public async Task UpdateAsync( SpecializationDto updatedSpec ) {
            var specialization = updatedSpec.Adapt<Specialization>();
            await _specializationRepository.UpdateAsync( specialization );

            await _publisher.Publish(
                new SpecializationUpdated() {
                    Id = specialization.Id,
                    IsActive = specialization.IsActive,
                    Name = specialization.Name,
                } );
        }
    }
}
