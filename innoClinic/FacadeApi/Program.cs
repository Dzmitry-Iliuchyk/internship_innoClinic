using Documents.GrpcApi;
using FacadeApi.Middleware;
using FacadeApi.Services;
using MassTransit;
using Serilog;
using Shared.Events.Contracts;
using Shared.PdfGenerator;
using SixLabors.ImageSharp;

var builder = WebApplication.CreateBuilder( args );
var config = builder.Configuration;
// Add services to the container.
builder.Services.Configure<ImageOptions>( config.GetSection( nameof( ImageOptions ) ) );
builder.Services.AddHttpClient( "results", cfg => {
    cfg.BaseAddress = new Uri( "http://appointments.api:8080" );
} );
builder.Services.AddHttpClient( "profiles", cfg => {
    cfg.BaseAddress = new Uri( "http://profiles.api:8080" );
} );
builder.Services.AddHttpClient( "offices", cfg => {
    cfg.BaseAddress = new Uri( "http://offices.api:8080" );
} );
builder.Services.AddGrpcClient<DocumentService.DocumentServiceClient>( "documents", cfg => {
    cfg.ChannelOptionsActions.Add( x => {
        x.MaxReceiveMessageSize = 100 * 1024 * 1024;
        x.MaxSendMessageSize = 100 * 1024 * 1024;   
        } );
    cfg.Address = new Uri( "http://documents.grpcapi:8080" );
} );
builder.Services.AddMassTransit( x => {
    x.SetKebabCaseEndpointNameFormatter();

    //x.AddConsumer<>();
    x.UsingRabbitMq( ( context, cfg ) => {
        cfg.Host( config[ "rabbitMq:host" ] ?? throw new ArgumentNullException( "rabbitMq:host" ),
            "/", h => {
                h.Username( config[ "rabbitMq:user" ] ?? throw new ArgumentNullException( "rabbitMq:user" ) );
                h.Password( config[ "rabbitMq:password" ] ?? throw new ArgumentNullException( "rabbitMq:password" ) );
            } );

        cfg.ConfigureEndpoints( context );
    } );
    
} );
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<PdfGeneratorService>();
builder.Services.AddSingleton<IMiddleware, ExceptionHandlingMiddleware>();
builder.Services.AddLogging( opt => {
    opt.ClearProviders();
    var logger = new LoggerConfiguration()
        .ReadFrom.Configuration( config )
        .CreateLogger();

    opt.AddSerilog( logger );
} );
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseMiddleware<ExceptionHandlingMiddleware>();
// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
