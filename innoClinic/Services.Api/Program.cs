using Microsoft.Extensions.DependencyInjection;
using Services.Api.Middlewares;
using Services.Application;
using Services.DataAccess;
using Services.Domain;
using System;

var builder = WebApplication.CreateBuilder( args );
var config = builder.Configuration;

// Add services to the container.
builder.Services.AddDataAccess(config);
builder.Services.AddApplication();
builder.Services.AddSingleton<ExceptionHandlingMiddleware>();
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
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
