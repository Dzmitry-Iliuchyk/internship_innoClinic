using Authorization.Application.Abstractions.Repositories;
using Authorization.Application.Abstractions.Services;
using Authorization.Application.Implementations;
using Authorrization.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Cryptography;

namespace Authorization.Application {
    public static class DependencyInjection {
        public static IServiceCollection ConfigureAuthApplicationDependncies( this IServiceCollection services) {
            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ITokenService, TokenService>();
            return services;
        }
    }
}
