namespace EventsWebApplication.Domain.Primitives;

public abstract class Entity
{
    protected Entity() { }
    public int Id { get; set; }
}
