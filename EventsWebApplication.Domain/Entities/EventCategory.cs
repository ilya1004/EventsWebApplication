using EventsWebApplication.Domain.Primitives;

namespace EventsWebApplication.Domain.Entities;

public class EventCategory : Entity
{
    public EventCategory(
        int id,
        string name,
        string normalizedName) : base(id)
    {
        Name = name;
        NormalizedName = normalizedName;
    }

    public string Name { get; set; } 
    public string NormalizedName { get; set; }
}
