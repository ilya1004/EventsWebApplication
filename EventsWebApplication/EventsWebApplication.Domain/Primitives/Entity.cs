namespace EventsWebApplication.Domain.Primitives;

public abstract class Entity
{
    protected Entity(int id)
    {
        Id = id;
    }
    public int Id { get; set; }
}
