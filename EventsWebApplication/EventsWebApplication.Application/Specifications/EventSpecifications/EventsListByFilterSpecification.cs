namespace EventsWebApplication.Application.Specifications.EventSpecifications;

public class EventsListByFilterSpecification : Specification<Event>
{
    public EventsListByFilterSpecification(
        DateTime? dateStart,
        DateTime? dateEnd,
        string? placeName,
        string? categoryName,
        int offset,
        int limit)
        : base(e =>
            (!dateStart.HasValue || !dateEnd.HasValue ||
                (dateStart.Value <= e.EventDateTime.Date && e.EventDateTime.Date <= dateEnd.Value.AddDays(1))) &&
            (string.IsNullOrEmpty(placeName) || 
                e.Place.Name.ToLower().Contains(placeName.ToLower())) &&
            (string.IsNullOrEmpty(categoryName) || 
                (e.Category != null && e.Category.Name.ToLower().Contains(categoryName.ToLower()))))
    {
        AddOrderBy(events => events.Id);

        AddPagination(offset, limit);
    }
}
