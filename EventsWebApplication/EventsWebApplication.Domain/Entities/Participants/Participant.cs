using EventsWebApplication.Domain.Entities.Events;
using EventsWebApplication.Domain.Primitives;

namespace EventsWebApplication.Domain.Entities.Participants;

public class Participant : Entity
{
    public Participant() : base(0) { }
    public Participant(
        string email,
        Person person,
        Event eventEntity) : base(0)
    {
        Email = email;
        Person = person;
        EventId = eventEntity.Id;
        EventRegistrationDate = DateTime.UtcNow;
    }

    public string Email { get; private set; }
    public Person Person { get; private set; }
    public int EventId { get; set; }    
    public Event? Event { get; set; }
    public DateTime EventRegistrationDate { get; set; }

}
