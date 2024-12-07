using EventsWebApplication.Domain.Primitives;
using System.Text.RegularExpressions;

namespace EventsWebApplication.Domain.Entities.Participants;

public sealed record Person(string Name, string Surname, DateTime BirthdayDate);
