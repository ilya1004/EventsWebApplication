using EventsWebApplication.API;
using EventsWebApplication.Application;
using EventsWebApplication.Infrastructure.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.AddControllers();

services.AddEndpointsApiExplorer()
        .AddSwaggerGen();

services.AddApplication();

services.AddPersistence(builder.Configuration);

services.AddAPI();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.MapControllers();

app.Run();
