using EventsWebApplication.Domain.Primitives;

namespace EventsWebApplication.Domain.Entities.Perticipants;

public class Participant : Entity
{
    public Participant(
        int id,
        string email,
        Person person,
        DateOnly eventRegistrationDate) : base(id)
    {
        Email = email;
        Person = person;
        EventRegistrationDate = eventRegistrationDate;
    }

    public string Email { get; set; }
    public Person Person { get; set; }
    public DateOnly EventRegistrationDate { get; set; }
}
