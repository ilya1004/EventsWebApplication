using EventsWebApplication.Domain.Entities.Events;
using EventsWebApplication.Domain.Primitives;

namespace EventsWebApplication.Domain.Entities.Participants;

public class Participant : Entity
{
    public Participant() : base(0) { }
    public Participant(
        string email,
        Person person,
        Event @event) : base(0)
    {
        Email = email;
        Person = person;
        //Event = @event;
        EventId = @event.Id;
        EventRegistrationDate = DateTime.UtcNow;
    }

    public string Email { get; private set; }
    public Person Person { get; private set; }
    public int EventId { get; set; }
    public Event? Event { get; private set; }
    public DateTime EventRegistrationDate { get; set; }

    public static Participant Create(
        string email,
        Event @event,
        Person person)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentNullException(nameof(email));
        }

        if (person == null)
        {
            throw new ArgumentNullException(nameof(person));
        }

        if (@event == null)
        {
            throw new ArgumentNullException(nameof(@event));
        }

        return new Participant(email, person, @event);

    }

    public void ChangeEmail(string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            throw new Exception("Email can't be empty");
        }
        Email = email;
    }

    public void ChangePersonalData(Person person)
    {
        if (person == null)
        {
            throw new Exception("Personal data can't be null");
        }

        if (string.IsNullOrEmpty(person.Name))
        {
            throw new Exception("Name can't be empty");
        }

        if (string.IsNullOrEmpty(person.Surname))
        {
            throw new Exception("Surname can't be empty");
        }

        if (person.BirthdayDate.AddYears(18) < DateTime.Today)
        {
            throw new Exception("Your age is under 18");
        }
    }
}
