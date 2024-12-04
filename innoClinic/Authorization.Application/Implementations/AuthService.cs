using Authorization.Application.Abstractions.Repositories;
using Authorization.Application.Abstractions.Services;
using Authorization.Application.Dtos;
using Authorization.Application.Exceptions;
using Authorization.Domain.Models.Enums;
using Authorrization.Api.Models;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace Authorization.Application.Implementations {
    public class AuthService: IAuthService {
        private readonly IUserRepository _userRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IPasswordHasher<User> _hasher;
        private readonly ITokenService _token;
        private readonly IValidator<User> _userValidator;
        private readonly IValidator<SignUpModel> _signUpValidator;
        public AuthService(
            IUserRoleRepository userRoleRepository,
            IUserRepository userRepository,
            IPasswordHasher<User> hasher,
            ITokenService tokenService,
            IValidator<User> userValidator,
            IValidator<SignUpModel> signUpValidator ) {

            _userRoleRepository = userRoleRepository;
            _userRepository = userRepository;
            _hasher = hasher;
            _token = tokenService;
            _userValidator = userValidator;
            _signUpValidator = signUpValidator;
        }

        public async Task AddRoleToUserAsync( Guid userId, int roleId ) {
            if (!await _userRepository.AnyAsync(userId)) {
                throw new UserNotFoundException( userId );
            }
            if (await _userRoleRepository.AnyAsync(userId, roleId )) {
                throw new UserRoleAlredyExistException( userId , roleId );
            }
            await _userRoleRepository.CreateAsync( new UserRole() {
                UserId = userId,
                RoleId = roleId
            } );
        }

        public async Task RemoveRoleFromUserAsync( Guid userId, int roleId ) {
            if (!await _userRepository.AnyAsync( userId )) {
                throw new UserNotFoundException( userId );
            }
            await _userRoleRepository.DeleteAsync( new UserRole() {
                UserId = userId,
                RoleId = roleId
            } );
        }

        public async Task<string> SignInAsync( SignInModel signUpModel ) {
            var userInDb = await _userRepository.GetUserWithRolesAsync( signUpModel.Email );
            if (userInDb == null) {
                throw new UserNotFoundException(signUpModel.Email);
            }
            var passwordVerificationResult = _hasher.VerifyHashedPassword( null, userInDb.PasswordHash, signUpModel.Password );
            if (passwordVerificationResult == PasswordVerificationResult.Failed ) {
                throw new WrongPasswordException( signUpModel.Email );
            }
            var token = _token.GenerateToken( userInDb );
            return token;
        }

        public async Task SignUpAsync( SignUpModel signUpModel ) {
            await _signUpValidator.ValidateAndThrowAsync(signUpModel);
            var userToCrate = new User() {
                Id = Guid.NewGuid(),
                Email = signUpModel.Email,
                PasswordHash = _hasher.HashPassword( null, signUpModel.Password )
            };

            if (await _userRepository.AnyAsync(signUpModel.Email)) {
                throw new UserAlredyExistException( signUpModel.Email );
            }

            await _userRepository.CreateAsync( userToCrate );
            await _userRoleRepository.CreateAsync( new UserRole {
                RoleId = (int)Roles.Patient,
                UserId = userToCrate.Id
            } );
        }
        public async Task<IEnumerable<string>> GetRoles(Guid userId) {
            if (!await _userRepository.AnyAsync(userId)) {
                throw new UserNotFoundException( userId );
            }
            var roles = await _userRepository.GetRolesAsync( userId );
            return roles?.Select(x=>x.Name)??[];
        }
        public async Task<IEnumerable<User>> GetUsers() {
            var users = await _userRepository.GetAllAsync( );
            return users??[];
        }
        public async Task<User> GetUserWithRoles(Guid userId) {
            if (!await _userRepository.AnyAsync( userId )) {
                throw new UserNotFoundException( userId );
            }
            var user = await _userRepository.GetUserWithRolesAsync( userId );
            return user;
        }
        public async Task<User> GetUserWithRoles(string email) {
            if (!await _userRepository.AnyAsync( email )) {
                throw new UserNotFoundException( email );
            }
            var user = await _userRepository.GetUserWithRolesAsync( email );
            return user;
        }
    }
}
