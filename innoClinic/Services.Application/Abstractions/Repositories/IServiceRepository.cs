using Services.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.Application.Abstractions.Repositories {
    public interface IServiceRepository {
        Task<bool> AnyAsync(Expression<Func<Service,bool>> filter);
        Task<Service?> GetAsync( Expression<Func<Service, bool>> filter );
        Task<IList<Service>> GetAllAsync();
        Task DeleteAsync( Service service );
        Task UpdateAsync(Service updatedService);
        Task CreateAsync(Service service);
        Task<Service?> GetLightAsync( Expression<Func<Service, bool>> filter );
    }
}
