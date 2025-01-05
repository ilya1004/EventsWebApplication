using System.Linq.Expressions;

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
                (e.EventDateTime.Date >= dateStart.Value && e.EventDateTime.Date <= dateEnd.Value)) &&
            (string.IsNullOrEmpty(placeName) || 
                e.Place.Name.ToLower().Contains(placeName.ToLower())) &&
            (string.IsNullOrEmpty(categoryName) || 
                (e.Category != null && e.Category.Name.ToLower().Contains(categoryName.ToLower()))))
    {
        ApplyPaging(offset, limit);

        ApplyOrderBy(events => events.OrderBy(e => e.Id));
    }
}
