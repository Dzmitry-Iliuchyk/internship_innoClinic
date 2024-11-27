using Authorization.DataAccess;

var builder = WebApplication.CreateBuilder( args );
var cfg = builder.Configuration;
// Add services to the container.
builder.Services.ConfigureAuthDataAccess(cfg.GetConnectionString( "Auth" ) );
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
using (var serviceScope = app.Services.CreateScope()) { 
    var context = serviceScope.ServiceProvider.GetService<AuthDbContext>();
    if (!context.Users.Any()) {
        context.SeedUsers(); 
    }
}
app.Run();
