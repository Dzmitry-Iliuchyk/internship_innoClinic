using Services.Domain;

namespace Services.Application.Abstractions.Repositories {
    public interface IServiceCategoryRepository {
        Task<ServiceCategory> GetAsync(int id);
        Task<IList<ServiceCategory>> GetAllAsync();
        Task DeleteAsync(int id);
        Task UpdateAsync(ServiceCategory updatedServiceCategory);
        Task<int> CreateAsync(ServiceCategory serviceCategory);
    }
}
