﻿using EventsWebApplication.Domain.Abstractions.EmailSenderService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace EventsWebApplication.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });

        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        services.AddFluentEmail(configuration["EmailSenderPapercut:EmailSender"], configuration["EmailSenderPapercut:SenderName"])
                .AddSmtpSender(configuration["EmailSenderPapercut:Host"], configuration.GetValue<int>("EmailSenderPapercut:Port"));

        return services;
    }
}
