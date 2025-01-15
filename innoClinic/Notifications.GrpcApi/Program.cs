using MassTransit;
using Notifications.Application;
using Notifications.GrpcApi.Services;
using Shared.Events.Contracts;

var builder = WebApplication.CreateBuilder( args );
var config = builder.Configuration;

// Add services to the container.

builder.Services.AddGrpc();
builder.Services.AddApplication(config);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<MailService>();
app.MapGet( "/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909" );
app.MapGet( "/test", async ( IRequestClient<SendEmailRequest> req ) => {
    var a = await req.GetResponse<SendEmailResponse>( new SendEmailRequest() {
        TextContent = " <html> <body> <h1>Привет!</h1> <p>Это простое HTML сообщение.</p> <p><b>С наилучшими пожеланиями,</b><br>Ваш отправитель</p> </body> </html>",
        NameFrom = "laboris ut",
        Subject = "occaecat reprehenderit fugiat",
        To = [
            "dima6061551@mail.ru",
            "dima6061551@gmail.com"
        ],
        File = new Shared.Events.Contracts.File() {
            FileName = "Iliuchyk Dzmitry_eng.pdf",
            FileContentType = "application/pdf",
            FileContent = System.IO.File.ReadAllBytes( "C:\\Users\\dima6\\Documents\\Iliuchyk Dzmitry_eng.pdf" )
        }
    } );
    Console.WriteLine( a.Message.Message );
} );
app.Run();
