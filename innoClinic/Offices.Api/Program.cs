
using Offices.DataAccess.DIConfiguration;
using Offices.Application.DIConfiguration;
using Serilog;

var builder = WebApplication.CreateBuilder( args );
var config = builder.Configuration;
// Add services to the container.
builder.Services.Configure<OfficesDatabaseSettings>( config.GetRequiredSection( "OfficesDatabaseSettings" ) );
builder.Services.AddOfficesDataAccess( config );
builder.Services.AddOfficesApplication();
builder.Services.AddLogging( opt => {
    opt.ClearProviders();

    var logger = new LoggerConfiguration()
        .ReadFrom.Configuration( config )
        .CreateLogger();

    opt.AddSerilog(logger);
} );

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
