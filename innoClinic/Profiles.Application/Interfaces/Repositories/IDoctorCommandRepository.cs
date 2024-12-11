using Profiles.Domain;
using Shared.Abstractions.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Profiles.Application.Interfaces.Repositories {
    public interface IDoctorCommandRepository {
        Task UpdateAsync( Doctor updatedEntity );
        Task CreateAsync( Doctor entity );
        Task DeleteAsync( Doctor entity );
    }
    public interface IDoctorReadRepository {
        Task<Doctor?> GetAsync( Guid id);
        Task<IList<Doctor>?> GetAllAsync();
    }
}
