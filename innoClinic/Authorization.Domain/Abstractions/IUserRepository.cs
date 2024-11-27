using Authorrization.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authorization.Domain.Repositories {
    public interface IUserRepository {
        Task Update(User updatedUser);
        Task Create(User user);
        Task Delete(User user);
        Task<User> Get(Guid userId);
        Task<IEnumerable<Role>> GetRoles(Guid userId);
        Task<IEnumerable<User>> GetAll();
    }
}
