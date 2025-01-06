namespace Services.Application.Abstractions.Services {
    public interface IServiceCategoryService {
        Task<ServiceCategoryDto?> GetAsync( int id );
        Task<IList<ServiceCategoryDto>> GetAllAsync();
        Task DeleteAsync( int serviceCategoryId );
        Task UpdateAsync( ServiceCategoryDto updatedServiceCategory );
        Task<int> CreateAsync( CreateServiceCategoryDto serviceCategory );
    }
}
