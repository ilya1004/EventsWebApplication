using EventsWebApplication.Domain.Entities.Events;

namespace EventsWebApplication.Domain.Abstractions.EmailSenderService;

public interface IEmailSenderService
{
    public Task SendEmailNotifications(Event oldEventData, Event newEventData, CancellationToken cancellationToken);
}
