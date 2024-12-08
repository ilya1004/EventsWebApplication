using EventsWebApplication.Domain.Entities.Participants;
using EventsWebApplication.Domain.Primitives;

namespace EventsWebApplication.Domain.Entities.Events;

public class Event : Entity
{
    private List<Participant> _participants = [];

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

    public static Event Create(
        string title,
        string? description,
        DateTime eventDateTime,
        int participantsMaxCount,
        string? image,
        string placeName,
        string? categoryName)
    {
        if (string.IsNullOrEmpty(title))
        {
            throw new ArgumentNullException(nameof(title));
        }


        if (eventDateTime < DateTime.UtcNow)
        {
            throw new Exception("DateTime of event can't be in the past");
        }

        if (string.IsNullOrEmpty(placeName))
        {
            throw new Exception("Place name can't be empty");
        }

        if (participantsMaxCount <= 0)
        {
            throw new Exception("Count of participants can't be equal or lower than 0");
        }

        var place = new Place(placeName, placeName.ToUpper());

        Category? category = null;
        if (categoryName != null) {
            category = new Category(categoryName, categoryName.ToUpper());
        }

        return new Event(title, description, eventDateTime, participantsMaxCount, image, place, category);
    }

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
