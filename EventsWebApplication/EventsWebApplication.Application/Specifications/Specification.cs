using EventsWebApplication.Domain.Abstractions.Specification;
using EventsWebApplication.Domain.Primitives;
using System.Linq.Expressions;

namespace EventsWebApplication.Application.Specifications;

public class Specification<TEntity> : ISpecification<TEntity> where TEntity : Entity
{
    public Expression<Func<TEntity, bool>> Criteria { get; }
    public Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? OrderBy { get; private set; }
    public Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? OrderByDescending { get; private set; }
    public List<Expression<Func<TEntity, object>>> Includes { get; } = [];
    public int? Take { get; private set; }
    public int? Skip { get; private set; }
    public bool IsPaginationEnabled { get; private set; }

    protected Specification(Expression<Func<TEntity, bool>> criteria)
    {
        Criteria = criteria;
    }

    protected void AddInclude(Expression<Func<TEntity, object>> includeExpression)
    {
        Includes.Add(includeExpression);
    }

    protected void ApplyPaging(int skip, int take)
    {
        Skip = skip;
        Take = take;
        IsPaginationEnabled = true;
    }

    protected void ApplyOrderBy(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderByExpression)
    {
        OrderBy = orderByExpression;
    }

    protected void ApplyOrderByDescending(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderByDescExpression)
    {
        OrderByDescending = orderByDescExpression;
    }
}
