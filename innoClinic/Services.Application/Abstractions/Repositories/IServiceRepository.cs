using Services.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Application.Abstractions.Repositories {
    public interface IServiceRepository {
        Task<Service?> GetAsync(Guid id);
        Task<IList<Service>> GetAllAsync();
        Task DeleteAsync(Guid id);
        Task UpdateAsync(Service updatedService);
        Task<Guid> CreateAsync(Service service);
    }
}
