using EventsWebApplication.Domain.Entities.Participants;
using EventsWebApplication.Domain.Primitives;

namespace EventsWebApplication.Domain.Entities.Events;

public class Event : Entity
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    public DateTime EventDateTime { get; set; }
    public int ParticipantsMaxCount { get; set; }
    public string? Image { get; set; }
    public Place Place { get; set; }
    public Category? Category { get; set; }
    public ICollection<Participant> Participants { get; set; } = [];
}