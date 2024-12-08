using Duende.IdentityServer.Services;
using EventsAppIdentityServer.API;
using EventsAppIdentityServer.API.Middlewares;
using EventsAppIdentityServer.API.Utils;
using EventsAppIdentityServer.Application;
using EventsAppIdentityServer.Application.Services;
using EventsAppIdentityServer.Domain.Abstractions;
using EventsAppIdentityServer.Domain.Entities;
using EventsAppIdentityServer.Infrastructure.Data;
using EventsAppIdentityServer.Infrastructure.DbInitializer;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresIdentityConnection")));

services.AddControllers();

services.AddAuthorization();
services.AddAuthentication();

services.AddTransient<GlobalExceptionHandlingMiddleware>();

services.AddTransient<IProfileService, MyProfileService>();

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
    .AddDeveloperSigningCredential()
    .AddProfileService<MyProfileService>();

services.AddApplication();
services.AddAPI();

services.AddEndpointsApiExplorer()
        .AddSwaggerGen();

services.AddScoped<IDbInitializer, DbInitializer>();

services.AddCors(options =>
{
    options.AddPolicy("ClientCorsPolicy", policy =>
    {
        policy.WithOrigins(builder.Configuration.GetSection("ReactClientUrl").Value!)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var app = builder.Build();

await SeedDatabase();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseCookiePolicy(new CookiePolicyOptions
//{
//    Secure = CookieSecurePolicy.Always,
//    HttpOnly = HttpOnlyPolicy.Always
//});


app.UseCors("ClientCorsPolicy");
app.UseIdentityServer();
app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.MapControllers();

app.MapIdentityApi<AppUser>();

app.Run();

async Task SeedDatabase()
{
    using var scope = app.Services.CreateScope();

    var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();

    await dbInitializer.InitializeDb();
}