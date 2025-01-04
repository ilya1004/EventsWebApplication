using EventsWebApplication.Domain.Entities.Events;
using EventsWebApplication.Domain.Primitives;

namespace EventsWebApplication.Domain.Entities.Participants;

public class Participant : Entity
{
    public string Email { get; set; } = string.Empty;
    public Person Person { get; set; }
    public int EventId { get; set; }    
    public Event? Event { get; set; }
    public DateTime EventRegistrationDate { get; set; }

}
