using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Profiles.Api;
using Profiles.Application;
using Profiles.Application.Common.Security;
using Profiles.DataAccess;
using Serilog;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

var builder = WebApplication.CreateBuilder( args );
var config = builder.Configuration;
var jwtOptions = config.GetRequiredSection( nameof( JwtOptions ) );
builder.Services.Configure<JwtOptions>( jwtOptions );
// Add services to the container.
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthentication( options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
} ).AddJwtBearer( options => {
    var credentials = GetKey( Path.Combine( Directory.GetCurrentDirectory(), "Auth","public_key.pem" ) );
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
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IIdentityService, IdentityService>();
builder.Services.AddDataAccess(config);
builder.Services.AddApplicationLayer();
builder.Services.AddControllers();
builder.Services.AddLogging( opt => {
    opt.ClearProviders();

    var logger = new LoggerConfiguration()
        .ReadFrom.Configuration( config )
        .CreateLogger();

    opt.AddSerilog( logger );
} );

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen( c => {
    c.SwaggerDoc( "v1", new OpenApiInfo { Title = "My API", Version = "v1" } );
    c.AddSecurityDefinition( "Bearer", new OpenApiSecurityScheme {
        In = ParameterLocation.Header,
        Description = "Введите токен JWT с префиксом Bearer. Пример: \"Bearer {token}\"",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    } );

    c.AddSecurityRequirement( new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    } );
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    c.IncludeXmlComments( Path.Combine( AppContext.BaseDirectory, xmlFilename ) );
} );

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
//}
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
using (var serviceScope = app.Services.CreateScope()) {
    var context = serviceScope.ServiceProvider.GetService<ProfilesDbContext>();
    context.Database.EnsureCreated();
    if (!context.Accounts.Any()) {
        context.EnsureSeedData();
    }
}
app.Run();

static RsaSecurityKey GetKey( string pathToKey ) {
    byte[] key = File.ReadAllBytes( pathToKey );
    var rsa = RSA.Create();
    rsa.ImportFromPem( Encoding.UTF8.GetChars( key ) );
    return new RsaSecurityKey( rsa );
}
