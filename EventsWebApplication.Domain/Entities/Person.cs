using EventsWebApplication.Domain.Primitives;
using System.Text.RegularExpressions;

namespace EventsWebApplication.Domain.Entities;

public class Person : Entity
{
    public Person(
        int id,
        string name,
        string surname,
        DateOnly birthdayDate) : base(id)
    {
        Name = name;
        Surname = surname;

        int age = DateTime.Today.Year - birthdayDate.Year;

        if (age < 18)
        {
            throw new Exception("Your age is under 18");
        }

        BirthdayDate = birthdayDate;
    }

    public string Name { get; set; }
    public string Surname { get; set; }
    public DateOnly BirthdayDate { get; set; }
}
