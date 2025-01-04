using System.Linq.Expressions;

namespace EventsWebApplication.Domain.Abstractions.Specification;

public interface ISpecification<TEntity>
{
    Expression<Func<TEntity, bool>> Criteria { get; }
    Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? OrderBy { get; }
    Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? OrderByDescending { get; }
    List<Expression<Func<TEntity, object>>> Includes { get; }
    int? Take { get; }
    int? Skip { get; }
    bool IsPaginationEnabled { get; }
}

