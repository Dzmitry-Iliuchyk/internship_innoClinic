using Authorization.Application.Abstractions.Services;
using Authorrization.Api.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace Authorization.Application.Implementations {
    public class TokenService: ITokenService {
        private readonly JwtOptions _options;
        public TokenService( IOptions<JwtOptions> options ) {
            _options = options.Value;
        }

        public string GenerateToken( User user ) {
            RsaSecurityKey securityKey = GetSecurityKey( Path.Combine(Directory.GetParent( Directory.GetCurrentDirectory() ).FullName, "Authorization.Application\\private_key.pem" ) );
            var credentials = new SigningCredentials( securityKey, SecurityAlgorithms.RsaSha256 );
            var claims = new List<Claim>() {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            };
            foreach (var role in user.Roles) {
                claims.Add( new Claim( ClaimTypes.Role, role.Name ) );
            }
            var token = new JwtSecurityToken(
                issuer: _options.Issuer,
                audience: _options.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes( _options.ExpiresMinutes ),
                signingCredentials:  credentials
                );

            var tokenValue = new JwtSecurityTokenHandler().WriteToken( token );
            return tokenValue;
        }

        public static RsaSecurityKey GetSecurityKey(string pathToKey) {
            byte[] privateKey = File.ReadAllBytes( pathToKey );
            var rsa = RSA.Create();
            rsa.ImportFromPem( Encoding.UTF8.GetChars( privateKey ) );
            return new RsaSecurityKey( rsa );
        }
    }
}
