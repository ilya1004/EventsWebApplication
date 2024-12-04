using EventsWebApplication.Domain.Primitives;

namespace EventsWebApplication.Domain.Entities.Perticipants;

public class Participant : Entity
{
    private Participant(
        int id,
        string email,
        Person person,
        DateTime eventRegistrationDate) : base(id)
    {
        Email = email;
        Person = person;
        EventRegistrationDate = eventRegistrationDate;
    }

    public string Email { get; private set; }
    public Person Person { get; private set; }
    public DateTime EventRegistrationDate { get; private set; }

    public static Participant Create(
        int id,
        string email,
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

        return new Participant(id, email, person, DateTime.UtcNow);

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
