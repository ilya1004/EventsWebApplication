using EventsWebApplication.Domain.Abstractions.Data;
using EventsWebApplication.Domain.Abstractions.EmailSenderService;
using FluentEmail.Core;

namespace EventsWebApplication.Application.Services;

public class EmailSenderService : IEmailSenderService
{
    private readonly IFluentEmail _fluentEmail;
    private readonly IUnitOfWork _unitOfWork;

    public EmailSenderService(IFluentEmail fluentEmail, IUnitOfWork unitOfWork)
    {
        _fluentEmail = fluentEmail;
        _unitOfWork = unitOfWork;
    }

    public async Task SendEmailNotifications(Event existingEventData, Event newEventData, CancellationToken cancellationToken)
    {
        var eventDateTimeChanged = existingEventData.EventDateTime != newEventData.EventDateTime;
        var eventPlaceChanged = existingEventData.Place.Name != newEventData.Place.Name;
        if (!eventDateTimeChanged && !eventDateTimeChanged)
        {
            return;
        }

        var eventParticipants = await _unitOfWork.ParticipantsRepository.ListAsync(p => p.EventId == existingEventData.Id);

        foreach (var participant in eventParticipants)
        {
            var message = BuildNotificationMessage(eventDateTimeChanged, eventPlaceChanged, newEventData);

            if (!string.IsNullOrEmpty(message))
            {
                var response = await _fluentEmail
                    .To(participant.Email)
                    .Subject($"Changes in event: {existingEventData.Title}")
                    .Body(message, isHtml: true)
                    .SendAsync(cancellationToken);

                if (!response.Successful)
                {
                    throw new Exception($"Email sending failed to {participant.Email}: {string.Join(", ", response.ErrorMessages)}");
                }
            }
        }
    }

    private string BuildNotificationMessage(bool dateTimeChanged, bool placeChanged, Event newEventData)
    {
        var changesHtml = new List<string>();

        if (dateTimeChanged)
        {
            changesHtml.Add($"<li>The event date and time has been changed to <b>{newEventData.EventDateTime:dd.MM.yyyy, HH:mm}</b>.</li>");
        }

        if (placeChanged)
        {
            changesHtml.Add($"<li>The event place has been updated to <b>{newEventData.Place.Name}</b>.</li>");
        }

        var body = $@"
            <p>Dear Event Participant</p>
            <p>We would like to inform you about some updates regarding the event <b>{newEventData.Title}</b>:</p>
            <ul>
                {string.Join("\n", changesHtml)}
            </ul>
            <p>Thank you for your attention!</p>
            <p>Your EventsWebApplication Team</p>";

        return body;
    }
}
