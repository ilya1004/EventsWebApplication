using EventsWebApplication.API;
using EventsWebApplication.API.Extensions;
using EventsWebApplication.API.Middlewares;
using EventsWebApplication.Application;
using EventsWebApplication.Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.AddControllers();

services.AddEndpointsApiExplorer()
        .AddSwaggerGen();

services.AddApplication(builder.Configuration);
services.AddPersistence(builder.Configuration);
services.AddAPI(builder.Configuration);

services.AddTransient<GlobalExceptionHandlingMiddleware>();

services.AddAuthenticationOptions(builder.Configuration);
services.AddAuthorizationPolicies();

services.AddCorsPolicies();

builder.Host.UseSerilog();

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(7012);
    // options.ListenAnyIP(7002, listenOptions => listenOptions.UseHttps()); 
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MakeMigrations();
    app.CreateContainerIfNotExist(builder.Configuration);
}

app.UseRouting();

app.UseCors("ReactClientCors");

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.MapControllers();

app.UseAuthentication();
app.UseAuthorization();


app.Run();

Log.CloseAndFlush();