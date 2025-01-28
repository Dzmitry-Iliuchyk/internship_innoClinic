using Appointments.Application;
using Appointments.DataAccess;
using Appointments.Middleware;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Security.Cryptography;
using System.Text;

var builder = WebApplication.CreateBuilder( args );
var config = builder.Configuration;
// Add services to the container.

var jwtOptions = config.GetRequiredSection( nameof( JwtOptions ) );
builder.Services.Configure<JwtOptions>( jwtOptions );
builder.Services.AddSingleton<ExceptionHandlingMiddleware>();

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
builder.Services.AddApplicationLayer();
builder.Services.AddDataAccess(config);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services
   .AddFastEndpoints()
   .SwaggerDocument();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();
//app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app
   .UseFastEndpoints( c => { 
       c.Serializer.Options.PropertyNamingPolicy = null; } )
   .UseSwaggerGen();

using (var scope = app.Services.CreateScope()) {
    var context = scope.ServiceProvider.GetRequiredService<AppointmentsDbContext>();
    context.Database.EnsureCreated();
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
