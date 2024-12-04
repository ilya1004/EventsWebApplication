﻿using EventsWebApplication.Infrastructure.Data;
using EventsWebApplication.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventsWebApplication.Infrastructure.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("PostgresConnection") ?? "Host=localhost; Port=5433; Database=eventsappdb; Username=postgres; Password=passpass";

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });

        services.AddSingleton<IUnitOfWork, AppUnitOfWork>();

        return services;
    }
}