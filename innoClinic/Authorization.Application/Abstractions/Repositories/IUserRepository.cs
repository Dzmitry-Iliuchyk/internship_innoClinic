using Authorrization.Api.Models;
using Shared.Abstractions.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authorization.Application.Abstractions.Repositories {
    public interface IUserRepository: IRepository<User> {

        Task<User?> GetAsync( Guid userId );
        Task<User?> GetAsync( string email );
        Task<IEnumerable<Role>?> GetRolesAsync( Guid userId );
        Task<User?> GetUserWithRolesAsync( Guid userId );
        Task<User?> GetUserWithRolesAsync( string email );
        Task<bool> AnyAsync(string email);
        Task<bool> AnyAsync(Guid userId);
    }
}
