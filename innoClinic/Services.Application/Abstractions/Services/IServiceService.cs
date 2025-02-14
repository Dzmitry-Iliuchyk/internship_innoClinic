using Services.Application.Abstractions.Services.Dtos;
using Services.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Application.Abstractions.Services {
    public interface IServiceService {
        Task<ServiceDto?> GetAsync( Guid id );
        Task<IList<ServiceDto>> GetAllAsync();
        Task DeleteAsync( Guid serviceId );
        Task UpdateAsync( UpdateServiceDto updatedService );
        Task<Guid> CreateAsync( CreateServiceDto service );
    }
}
