using Profiles.Domain;
using System.Linq.Expressions;

namespace Profiles.Application.Interfaces.Repositories {
    public interface IPatientCommandRepository {
        Task UpdateAsync( Patient updatedEntity );
        Task CreateAsync( Patient entity );
        Task DeleteAsync( Patient entity );
    }
    public interface IPatientReadRepository {
        Task<Patient?> GetAsync( Guid id );
        Task<IList<Patient>?> GetAllAsync();
    }
    public interface ISpecializationRepository {
        Task<Specialization?> GetAsync( Expression<Func<Specialization, bool>> filter );
        Task<IList<Specialization>> GetAllAsync();
        Task DeleteAsync( Specialization specialization );
        Task UpdateAsync( Specialization updatedSpecialization );
        Task CreateAsync( Specialization specialization );
    }

}
