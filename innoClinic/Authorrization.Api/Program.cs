using Authorization.Api.Middleware;
using Authorization.Application;
using Authorization.Application.Implementations;
using Authorization.DataAccess;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;


var builder = WebApplication.CreateBuilder( args );
var cfg = builder.Configuration;
var jwtOptions = cfg.GetRequiredSection( nameof( JwtOptions ) );
builder.Services.Configure<JwtOptions>( jwtOptions );
// Add services to the container.

var credentials = TokenService.GetSecurityKey( Path.Combine( Directory.GetParent(Directory.GetCurrentDirectory()).FullName, "Authorization.Application\\public_key.pem" ) );

builder.Services.AddAuthentication( options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
} ).AddJwtBearer( options => {
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
builder.Services.AddScoped<ExceptionHandlingMiddleware>();
builder.Services.AddAuthorization();
builder.Services.ConfigureAuthDataAccess( cfg.GetConnectionString( "Auth" ) );
builder.Services.ConfigureAuthApplicationDependncies();
builder.Services.AddControllers();
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
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
using (var serviceScope = app.Services.CreateScope()) {
    var context = serviceScope.ServiceProvider.GetService<AuthDbContext>();
    if (!context.Users.Any()) {
        context.SeedUsers();
    }
}
app.Run();
