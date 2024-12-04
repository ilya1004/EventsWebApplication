using EventsWebApplication.Domain.Primitives;
using System.Text.RegularExpressions;

namespace EventsWebApplication.Domain.Entities.Perticipants;

public sealed record Person(string Name, string Surname, DateOnly BirthdayDate);
