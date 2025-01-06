using Services.Domain;

namespace Services.Application.Abstractions.Services {
    public interface ISpecializationService {
        Task<SpecializationDto?> GetAsync( int id );
        Task<IList<string>> GetAllAsync();
        Task DeleteAsync( int specializationId );
        Task UpdateAsync( SpecializationDto updatedSpecialization );
        Task<int> CreateAsync( CreateSpecializationDto specialization );
    }
}
