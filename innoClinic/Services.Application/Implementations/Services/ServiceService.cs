using Mapster;
using Services.Application.Abstractions.Repositories;
using Services.Application.Abstractions.Services;
using Services.Application.Exceptions;
using Services.Domain;

namespace Services.Application.Implementations.Services {
    public class ServiceService: IServiceService {
        private readonly IServiceRepository _serviceRepository;
        private readonly ISpecializationRepository _specializationRepository;
        private readonly IServiceCategoryRepository _serviceCategoryRepository;
        public ServiceService( IServiceRepository serviceRepository, ISpecializationRepository specializationRepository, IServiceCategoryRepository serviceCategoryrepository ) {
            this._serviceRepository = serviceRepository;
            this._specializationRepository = specializationRepository;
            this._serviceCategoryRepository = serviceCategoryrepository;
        }
        public async Task<Guid> CreateAsync( CreateServiceDto service ) {
            if (await _serviceRepository.AnyAsync( x => x.Name == service.Name )) {
                throw new ServiceAlreadyExistException(service.Name);
            }
            var serviceToCreate = service.Adapt<Service>();
            serviceToCreate.Id = Guid.NewGuid();
            await FillServiceEntity( serviceToCreate );
            await _serviceRepository.CreateAsync( serviceToCreate );
            return serviceToCreate.Id;
        }
        public async Task DeleteAsync( Guid serviceId ) {
            var service = await _serviceRepository.GetLightAsync( x => x.Id == serviceId );
            if (service == null)
                throw new ServiceNotFoundException(serviceId);
            await _serviceRepository.DeleteAsync( service );
        }
        public async Task<IList<ServiceDto>> GetAllAsync() {
            return ( await _serviceRepository.GetAllAsync() ).Adapt<List<ServiceDto>>();
        }
        public async Task<ServiceDto?> GetAsync( Guid id ) {
            return (( await _serviceRepository.GetAsync( x => x.Id == id ) )
                ??throw new ServiceNotFoundException(id))
                    .Adapt<ServiceDto>();
        }
        public async Task UpdateAsync( UpdateServiceDto updatedService ) {
            if (!await _serviceRepository.AnyAsync( x => x.Id == updatedService.Id ))
                throw new ServiceNotFoundException(updatedService.Id);
            var itemToUpdate = updatedService.Adapt<Service>();
            await FillServiceEntity( itemToUpdate );
            await _serviceRepository.UpdateAsync( itemToUpdate );
        }

        private async Task FillServiceEntity( Service service ) {
            var category = (await _serviceCategoryRepository.GetAsync( x => x.Name == service.Category.Name ))
                ?? throw new ServiceCategoryNotFoundException(service.Category.Name);
            var spec = (await _specializationRepository.GetAsync( x => x.Name == service.Specialization.Name ))
                ?? throw new SpecializationNotFoundException( service.Specialization.Name );
            
            service.SpecializationId = spec.Id;
            service.Specialization = null;
            service.CategoryId = category.Id;
            service.Category = null;
        }

    }
}
