
using EventsWebApplication.Domain.Abstractions.Specification;
using Microsoft.EntityFrameworkCore;

namespace EventsWebApplication.Application.Specifications;

public static class SpecificationEvaluator<TEntity> where TEntity : class
{
    public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecification<TEntity> specification)
    {
        var query = inputQuery;

        if (specification.Criteria is not null)
        {
            query = query.Where(specification.Criteria);
        }

        query = specification.IncludesExpression.Aggregate(query,
            (current, include) => current.Include(include));

        if (specification.OrderByExpression is not null)
        {
            query = query.OrderBy(specification.OrderByExpression);
        }
        else if (specification.OrderByDescExpression is not null)
        {
            query = query.OrderByDescending(specification.OrderByDescExpression);
        }

        if (specification.IsPaginationEnabled)
        {
            if (specification.Skip.HasValue)
            {
                query = query.Skip(specification.Skip.Value);
            }

            if (specification.Take.HasValue)
            {
                query = query.Take(specification.Take.Value);
            }
        }

        return query;
    }
}
