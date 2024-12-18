using Services.Domain;
using System.Linq.Expressions;

namespace Services.Application.Abstractions.Repositories {
    public interface IServiceCategoryRepository {
        Task<ServiceCategory?> GetAsync( Expression<Func<ServiceCategory, bool>> filter );
        Task<IList<ServiceCategory>> GetAllAsync();
        Task DeleteAsync( ServiceCategory serviceCategory );
        Task UpdateAsync( ServiceCategory updatedServiceCategory );
        Task<int> CreateAsync( ServiceCategory serviceCategory );
        Task<ServiceCategory?> GetLightAsync( Expression<Func<ServiceCategory, bool>> filter );
        Task<bool> AnyAsync( Expression<Func<ServiceCategory, bool>> filter );
    }
}
