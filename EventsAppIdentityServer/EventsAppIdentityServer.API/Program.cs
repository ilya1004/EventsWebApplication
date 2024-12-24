using EventsAppIdentityServer.API;
using EventsAppIdentityServer.API.Extensions;
using EventsAppIdentityServer.API.Middlewares;
using EventsAppIdentityServer.Application;
using EventsAppIdentityServer.Domain.Entities;
using EventsAppIdentityServer.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddControllers();

services.AddTransient<GlobalExceptionHandlingMiddleware>();

services.AddApplication();
services.AddAPI();
services.AddInfrastructure(builder.Configuration);

services.AddIdentityConfiguration(builder.Configuration);

services.AddEndpointsApiExplorer()
        .AddSwaggerGen();

services.AddCorsPolicies(builder.Configuration);

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MakeMigrations();
}

await app.SeedDatabase();

app.UseCors("CorsPolicy");

app.UseIdentityServer();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.MapControllers();

app.MapIdentityApi<AppUser>();

app.Run();
