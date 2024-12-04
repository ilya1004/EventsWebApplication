using EventsWebApplication.Domain.Entities.Perticipants;
using EventsWebApplication.Domain.Primitives;

namespace EventsWebApplication.Domain.Entities.Events;

public class Event : Entity
{
    private List<Participant> _participants = [];
    public Event(
        int id,
        string title,
        string description,
        DateTime eventDateTime,
        Place place,
        int participantsMaxCount,
        string image,
        Category? category) : base(id)
    {
        Title = title;
        Description = description;
        EventDateTime = eventDateTime;
        Place = place;
        ParticipantsMaxCount = participantsMaxCount;
        Image = image;
        Category = category;
        _participants = [];
    }

    public string Title { get; private set; } = string.Empty;
    public string? Description { get; private set; } = string.Empty;
    public DateTime EventDateTime { get; private set; }
    public int ParticipantsMaxCount { get; private set; }
    public string? Image { get; private set; }
    public Place Place { get; private set; }
    public Category? Category { get; private set; }
    public IReadOnlyList<Participant> Participants { get => _participants.AsReadOnly(); }

    public void AddParticipant(Participant participant)
    {
        if (_participants.Count >= ParticipantsMaxCount)
        {
            throw new InvalidOperationException("Maximum number of participants reached.");
        }

        _participants.Add(participant);
    }

    public void RemoveParticipant(Participant participant)
    {
        if (!_participants.Remove(participant))
        {
            throw new InvalidOperationException("Participant not found.");
        }
    }

    public void UpdateEvent(string title, string? description, DateTime eventDateTime, Place place, string? image, Category? category)
    {
        Title = title;
        Description = description;
        EventDateTime = eventDateTime;
        Place = place;
        Image = image;
        Category = category;
    }
}
