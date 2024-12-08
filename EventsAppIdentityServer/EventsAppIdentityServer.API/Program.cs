
using EventsAppIdentityServer.API.Utils;
using EventsAppIdentityServer.Domain.Abstractions;
using EventsAppIdentityServer.Domain.Entities;
using EventsAppIdentityServer.Infrastructure.Data;
using EventsAppIdentityServer.Infrastructure.DbInitializer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresIdentityConnection")));

services.AddAuthorization();
services.AddAuthentication();

services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddApiEndpoints()
    .AddDefaultTokenProviders();

services.AddIdentityServer(option =>
{
    option.Events.RaiseErrorEvents = true;
    option.Events.RaiseInformationEvents = true;
    option.Events.RaiseFailureEvents = true;
    option.Events.RaiseSuccessEvents = true;
    option.EmitStaticAudienceClaim = true;
})
    .AddInMemoryIdentityResources(UtilsProvider.IdentityResources)
    .AddInMemoryApiScopes(UtilsProvider.ApiScopes)
    .AddInMemoryClients(UtilsProvider.Clients)
    .AddAspNetIdentity<AppUser>()
    .AddDeveloperSigningCredential();
 
    //.AddDeveloperSigningCredential();

services.AddEndpointsApiExplorer()
    .AddSwaggerGen();

services.AddScoped<IDbInitializer, DbInitializer>();

var app = builder.Build();

await SeedDatabase();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapIdentityApi<AppUser>();

app.Run();

async Task SeedDatabase()
{
    using var scope = app.Services.CreateScope();

    var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();

    await dbInitializer.InitializeDb();
}