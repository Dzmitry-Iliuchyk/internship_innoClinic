using Services.Domain;

namespace Services.Application.Abstractions.Repositories {
    public interface ISpecializationRepository {
        Task<Specialization> GetAsync(int id);
        Task<IList<Specialization>> GetAllAsync();
        Task DeleteAsync(int id);
        Task UpdateAsync(Specialization updatedSpecialization);
        Task<int> CreateAsync(Specialization specialization);
    }
}
