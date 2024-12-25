using EventsWebApplication.API.Utils;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Serilog;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using System.Reflection;
using System.Security.Claims;

namespace EventsWebApplication.API;

public static class DependencyInjection
{
    public static IServiceCollection AddAPI(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(config =>
        {
            config.AddMaps(Assembly.GetExecutingAssembly());
        });

        
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddFluentValidationAutoValidation(config =>
        {
            config.EnableFormBindingSourceAutomaticValidation = true;
            config.EnableBodyBindingSourceAutomaticValidation = true;
        });


        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();

        return services;
    }

    public static IServiceCollection AddAuthenticationOptions(this IServiceCollection services, IConfiguration configuration)
    {
        var identityBase = configuration["AUTHORITY_URI"] ?? "http://localhost:7013";

        services.AddAuthentication(options =>
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

        return services;
    }

    public static IServiceCollection AddAuthorizationPolicies(this IServiceCollection services)
    {
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

        return services;
    }

    public static IServiceCollection AddCorsPolicies(this IServiceCollection services)
    {
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
        
        return services;
    }
}
