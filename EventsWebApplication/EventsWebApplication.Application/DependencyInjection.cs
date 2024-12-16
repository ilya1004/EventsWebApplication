using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace EventsWebApplication.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });

        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        services.AddHttpClient("IdentityClient", client =>
        {
            client.BaseAddress = new Uri("http://eventwebapp_identityserver:7013");
            //client.BaseAddress = new Uri(configuration.GetRequiredSection("ServiceUrls:IdentityAPI").Value ?? "https://localhost:7003/");
        });

        services.AddFluentEmail(configuration["EmailSenderPapercut:EmailSender"], configuration["EmailSenderPapercut:SenderName"])
                .AddSmtpSender(configuration["EmailSenderPapercut:Host"], configuration.GetValue<int>("EmailSenderPapercut:Port"));

        return services;
    }
}
