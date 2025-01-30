using Documents.DataAccess;
using Documents.GrpcApi.Interceptors;
using Documents.GrpcApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Text;
using Shared.ServiceDiscovery;

var builder = WebApplication.CreateBuilder( args );
var config = builder.Configuration;
var jwtOptions = config.GetRequiredSection( nameof( JwtOptions ) );
builder.Services.Configure<JwtOptions>( jwtOptions );
// Add services to the container.
builder.Services.AddAuthentication( options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
} ).AddJwtBearer( options => {
    var credentials = GetKey( Path.Combine( Directory.GetCurrentDirectory(), "Auth", "public_key.pem" ) );
    options.TokenValidationParameters = new TokenValidationParameters {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero,
        ValidIssuer = jwtOptions.GetValue<string>( nameof( JwtOptions.Issuer ) ),
        ValidAudience = jwtOptions.GetValue<string>( nameof( JwtOptions.Audience ) ),
        IssuerSigningKey = credentials
    };
} );
builder.Services.AddAuthorization();
// Add services to the container.
builder.Services.AddDataAccess(config);
builder.Services.AddGrpc( opt => {
    opt.MaxReceiveMessageSize = 100 * 1024 * 1024;
    opt.MaxSendMessageSize = 100 * 1024 * 1024;
    opt.Interceptors.Add<ExceptionHandlingInterceptor>();
} );
ConfigureConsul( builder.Services );
var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();
// Configure the HTTP request pipeline.
app.MapGrpcService<HealthCheckService>();
app.MapGrpcService<DocumentService>();
app.MapGet( "/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909" );

app.Run();

static RsaSecurityKey GetKey( string pathToKey ) {
    byte[] key = File.ReadAllBytes( pathToKey );
    var rsa = RSA.Create();
    rsa.ImportFromPem( Encoding.UTF8.GetChars( key ) );
    return new RsaSecurityKey( rsa );
}
void ConfigureConsul( IServiceCollection services ) {
    var serviceConfig = config.GetServiceConfig();

    services.RegisterConsulServices( serviceConfig );
}