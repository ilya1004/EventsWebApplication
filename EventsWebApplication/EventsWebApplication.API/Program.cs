using EventsWebApplication.API;
using EventsWebApplication.API.Extensions;
using EventsWebApplication.API.Middlewares;
using EventsWebApplication.API.Utils;
using EventsWebApplication.Application;
using EventsWebApplication.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Security.Claims;
using static System.Net.WebRequestMethods;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.AddControllers();

services.AddEndpointsApiExplorer()
        .AddSwaggerGen();

services.AddApplication(builder.Configuration);
services.AddPersistence(builder.Configuration);
services.AddAPI(builder.Configuration);

services.AddTransient<GlobalExceptionHandlingMiddleware>();

var identityBase = builder.Configuration["AUTHORITY_URI"] ?? "http://localhost:7013";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.Authority = identityBase;
        options.RequireHttpsMetadata = false;
        options.Audience = $"{identityBase}/resources";
    });



services.AddAuthorizationBuilder()
    .AddPolicy(AuthPolicies.UserPolicy, policy =>
    {
        policy.RequireRole("User");
        policy.RequireClaim(ClaimTypes.NameIdentifier);
    })
    .AddPolicy(AuthPolicies.AdminPolicy, policy =>
    {
         policy.RequireRole("Admin");
    })
    .AddPolicy(AuthPolicies.AdminOrUserPolicy, policy =>
    {
        policy.RequireRole("Admin", "User");
    });

services.AddCors(options =>
{
    options.AddPolicy("ReactClientCors", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "http://eventwebapp.client:3000")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});


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