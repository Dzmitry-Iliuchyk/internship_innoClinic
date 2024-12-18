using Services.Domain;
using System.Linq.Expressions;

namespace Services.Application.Abstractions.Repositories {
    public interface ISpecializationRepository {
        Task<Specialization?> GetAsync( Expression<Func<Specialization, bool>> filter );
        Task<IList<Specialization>> GetAllAsync();
        Task DeleteAsync( Specialization specialization );
        Task UpdateAsync(Specialization updatedSpecialization);
        Task<int> CreateAsync(Specialization specialization);
        Task<Specialization?> GetLightAsync( Expression<Func<Specialization, bool>> filter );
        Task<bool> AnyAsync( Expression<Func<Specialization, bool>> filter );
    }
}
