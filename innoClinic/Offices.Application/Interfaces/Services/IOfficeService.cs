using Offices.Application.Dtos;

namespace Offices.Application.Interfaces.Services {
    public interface IOfficeService {
        Task<OfficeDto> GetAsync( string id );
        Task<List<OfficeDto>> GetAllAsync();
        Task UpdateAsync(string id, UpdateOfficeDto officeDto );
        Task DeleteAsync( string id );
        Task<string> CreateAsync( CreateOfficeDto officeDto );
    }
}
