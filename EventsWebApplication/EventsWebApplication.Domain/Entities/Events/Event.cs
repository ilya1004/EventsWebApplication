using EventsWebApplication.Domain.Entities.Participants;
using EventsWebApplication.Domain.Primitives;

namespace EventsWebApplication.Domain.Entities.Events;

public class Event : Entity
{
    private readonly List<Participant> _participants = [];

    public Event() : base(0) { }
    public Event(
       string title,
       string? description,
       DateTime eventDateTime,
       int participantsMaxCount,
       string? image,
       Place place,
       Category? category) : base(0)
    {
        Title = title;
        Description = description;
        EventDateTime = eventDateTime;
        ParticipantsMaxCount = participantsMaxCount;
        Image = image;
        Place = place;
        Category = category;
    }

    public string Title { get; private set; } = string.Empty;
    public string? Description { get; private set; } = string.Empty;
    public DateTime EventDateTime { get; private set; }
    public int ParticipantsMaxCount { get; private set; }
    public string? Image { get; set; }
    public Place Place { get; private set; }
    public Category? Category { get; private set; }
    public IReadOnlyList<Participant> Participants { get => _participants.AsReadOnly(); }

}
