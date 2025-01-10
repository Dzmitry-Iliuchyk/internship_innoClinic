using Appointments.Application.Dtos;

namespace Appointments.Application.Interfaces.Services {
    public interface IResultService {
        Task<ResultFullDto> GetAsync( Guid id );
        Task<Guid> CreateAsync( ResultCreateDto entity );
        Task UpdateAsync( ResultDto entity );
        Task DeleteAsync( Guid entity );
        Task<IList<ResultDto>> GetAllAsync();
    }
}
