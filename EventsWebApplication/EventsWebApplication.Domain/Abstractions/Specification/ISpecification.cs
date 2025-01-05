using System.Linq.Expressions;

namespace EventsWebApplication.Domain.Abstractions.Specification;

public interface ISpecification<TEntity>
{
    Expression<Func<TEntity, bool>> Criteria { get; }
    List<Expression<Func<TEntity, object>>> IncludesExpression { get; }
    Expression<Func<TEntity, object>> OrderByExpression { get; }
    Expression<Func<TEntity, object>> OrderByDescExpression { get; }
    int? Take { get; }
    int? Skip { get; }
    bool IsPaginationEnabled { get; }
}

