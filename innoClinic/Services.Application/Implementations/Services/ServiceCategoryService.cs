using Mapster;
using Services.Application.Abstractions.Repositories;
using Services.Application.Abstractions.Services;
using Services.Application.Exceptions;
using Services.Domain;

namespace Services.Application.Implementations.Services {
    public class ServiceCategoryService: IServiceCategoryService {
        private readonly IServiceCategoryRepository _serviceCategoryRepository;

        public ServiceCategoryService( IServiceCategoryRepository serviceCategoryRepository ) {
            this._serviceCategoryRepository = serviceCategoryRepository;

        }

        public async Task<int> CreateAsync( CreateServiceCategoryDto serviceCategory ) {
            if (await _serviceCategoryRepository.AnyAsync( x => x.Name == serviceCategory.Name)) {
                throw new ServiceCategoryAlreadyExistException( serviceCategory.Name );
            }
            return await _serviceCategoryRepository.CreateAsync( serviceCategory.Adapt<ServiceCategory>() );
        }

        public async Task DeleteAsync( int serviceCategoryId ) {
            var serviceCategory = await _serviceCategoryRepository.GetLightAsync( x => x.Id == serviceCategoryId );
            if (serviceCategory == null) {
                throw new ServiceCategoryNotFoundException(serviceCategoryId);
            }
            await _serviceCategoryRepository.DeleteAsync( serviceCategory );
        }

        public async Task<IList<ServiceCategoryDto>> GetAllAsync() {
            return ( await _serviceCategoryRepository.GetAllAsync() ).Adapt<List<ServiceCategoryDto>>();
        }

        public async Task<ServiceCategoryDto?> GetAsync( int id ) {
            if (!await _serviceCategoryRepository.AnyAsync( x => x.Id == id )) {
                throw new ServiceCategoryNotFoundException( id );
            }
            return ( await _serviceCategoryRepository.GetAsync( x => x.Id == id ) ).Adapt<ServiceCategoryDto?>();
        }

        public async Task UpdateAsync( ServiceCategoryDto updatedServiceCategory ) {
            if (!await _serviceCategoryRepository.AnyAsync(x=>x.Id == updatedServiceCategory.Id )) {
                throw new ServiceCategoryNotFoundException( updatedServiceCategory.Id );
            } 
            if (await _serviceCategoryRepository.AnyAsync(x=>x.Name == updatedServiceCategory.Name 
                                                            && x.Id != updatedServiceCategory.Id )) {
                throw new ServiceCategoryAlreadyExistException( updatedServiceCategory.Name );
            }
            await _serviceCategoryRepository.UpdateAsync( updatedServiceCategory.Adapt<ServiceCategory>() );
        }
    }
}
