using EventsWebApplication.Domain.Primitives;

namespace EventsWebApplication.Domain.Entities;

public class Participant : Entity
{
    public Participant(
        int id,
        string email,
        Person person,
        DateOnly eventRegistrationDate) : base(id)
    {
        Email = email;
        PersonId = person.Id;
        Person = person;
        EventRegistrationDate = eventRegistrationDate;
    }

    public string Email { get; set; }
    public int PersonId { get; set; }
    public Person Person { get; set; }
    public DateOnly EventRegistrationDate { get; set; }   
}
