using Profiles.Domain;
using System.Linq.Expressions;

namespace Profiles.Application.Interfaces.Repositories {
    public interface IReceptionistCommandRepository {
        Task UpdateAsync( Receptionist updatedEntity );
        Task CreateAsync( Receptionist entity );
        Task DeleteAsync( Receptionist entity );
    }    
    public interface IReceptionistReadRepository {
        Task<Receptionist?> GetAsync( Guid id );
        Task<IList<Receptionist>?> GetAllAsync();
    }
}
