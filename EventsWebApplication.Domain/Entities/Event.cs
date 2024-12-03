using EventsWebApplication.Domain.Primitives;

namespace EventsWebApplication.Domain.Entities;

public class Event : Entity
{
    public Event(
        int id,
        string title,
        string description,
        DateTime eventDateTime,
        EventPlace place,
        int participantsMaxCount,
        string image,
        EventCategory? category) : base(id)
    {
        Title = title;
        Description = description;
        EventDateTime = eventDateTime;
        PlaceId = place.Id;
        Place = place;
        ParticipantsMaxCount = participantsMaxCount;
        Image = image;

        if (category != null)
        {
            CategoryId = category.Id;
        }

        Category = category;
        Participants = [];
    }

    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    public DateTime EventDateTime { get; set; }
    public int PlaceId { get; set; }
    public EventPlace Place { get; set; }
    public int ParticipantsMaxCount { get; set; }
    public string? Image { get; set; }
    public int? CategoryId { get; set; }
    public EventCategory? Category { get; set; }
    public IEnumerable<Participant> Participants { get; set; } = [];
}
