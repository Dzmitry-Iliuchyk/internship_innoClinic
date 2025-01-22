using Documents.GrpcApi;

var builder = WebApplication.CreateBuilder( args );

// Add services to the container.

builder.Services.AddHttpClient( "profiles", cfg => {
    cfg.BaseAddress = new Uri( "http://profiles.api:8080" );
} );
builder.Services.AddHttpClient( "offices", cfg => {
    cfg.BaseAddress = new Uri( "http://offices.api:8080" );
} );
builder.Services.AddGrpcClient<DocumentService.DocumentServiceClient>( "documents", cfg => {
    cfg.Address = new Uri( "http://documents.grpcapi:8080" );
} );
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
