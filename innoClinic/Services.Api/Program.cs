using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Services.Api.Middlewares;
using Services.Application;
using Services.DataAccess;
using Services.Domain;
using System.Security.Cryptography;
using Shared.ServiceDiscovery;
using System.Text;

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
builder.Services.AddDataAccess(config);
builder.Services.AddApplication(config);
builder.Services.AddSingleton<ExceptionHandlingMiddleware>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
ConfigureConsul( builder.Services );
var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
//}
app.UseMiddleware<ExceptionHandlingMiddleware>();

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
using (var scope = app.Services.CreateScope()) {
    var context = scope.ServiceProvider.GetRequiredService<ServiceContext>();

    if (context.Database.GetPendingMigrations().Any()) {
        context.Database.Migrate();
    }
}
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