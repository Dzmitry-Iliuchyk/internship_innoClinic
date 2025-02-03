using Authorization.Application.Dtos;
using Authorrization.Api.Models;

namespace Authorization.Application.Abstractions.Services {
    public interface IAuthService {
        public Task SignUpAsync( SignUpModel signUpModel );
        public Task<string> SignInAsync( SignInModel signUpModel );
        public Task AddRoleToUserAsync(Guid userId, int roleId);
        public Task RemoveRoleFromUserAsync( Guid userId, int roleId );
        Task<IEnumerable<string>> GetRoles( Guid userId );
        Task<IEnumerable<User>> GetUsers();
        Task<User> GetUserWithRoles( Guid userId );
        Task<User> GetUserWithRoles( string email );
        Task UpdateEmailAsync( UpdateEmailModel updateModel );
        Task UpdatePasswordAsync( UpdatePasswordModel updateModel );
        Task CreateAccountAsync( CreateAccountModel createAccountModel );
        Task DeleteAccountAsync( Guid id );
    }
}
