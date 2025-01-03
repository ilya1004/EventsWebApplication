﻿using FluentValidation;
using System.Reflection;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using EventsAppIdentityServer.API.Utils;
using EventsAppIdentityServer.Application.Services;
using EventsAppIdentityServer.Domain.Entities;
using EventsAppIdentityServer.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;

namespace EventsAppIdentityServer.API;

public static class DependencyInjection
{
    public static IServiceCollection AddAPI(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddFluentValidationAutoValidation(config =>
        {
            config.EnableFormBindingSourceAutomaticValidation = true;
            config.EnableBodyBindingSourceAutomaticValidation = true;
        });
        
        return services;
    }

    public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
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
            option.IssuerUri = configuration["ISSUER_URI"] ?? "http://localhost:7013";
        })
            .AddInMemoryIdentityResources(IdentityConfigurationProvider.IdentityResources)
            .AddInMemoryApiScopes(IdentityConfigurationProvider.ApiScopes)
            .AddInMemoryClients(IdentityConfigurationProvider.Clients)
            .AddAspNetIdentity<AppUser>()
            .AddDeveloperSigningCredential()
            .AddProfileService<MyProfileService>();

        return services;
    }

    public static IServiceCollection AddCorsPolicies(this IServiceCollection services, IConfiguration configuration)
    {
        var reactClientUrl = configuration["ReactClientUrl"] ?? "http://localhost:3000";

        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", policy =>
            {
                policy.WithOrigins(reactClientUrl)
                      .AllowAnyHeader()
                      .AllowAnyMethod()
                      .AllowCredentials();
            });
        });

        return services;
    }
}
