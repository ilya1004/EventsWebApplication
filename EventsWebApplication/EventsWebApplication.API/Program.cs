using EventsWebApplication.API;
using EventsWebApplication.API.Middlewares;
using EventsWebApplication.Application;
using EventsWebApplication.Domain.Abstractions.StartupServices;
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
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var migrationsStartupService = scope.ServiceProvider.GetRequiredService<IMigrationsStartupService>();
    await migrationsStartupService.MakeMigrationsAsync();

    var azuriteStartupService = scope.ServiceProvider.GetRequiredService<IAzuriteStartupService>();
    await azuriteStartupService.CreateContainerIfNotExistAsync();
    await azuriteStartupService.SeedImagesAsync();
}

app.UseRouting();

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.UseCors("ReactClientCors");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

Log.CloseAndFlush();