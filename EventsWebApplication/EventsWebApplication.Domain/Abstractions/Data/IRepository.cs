using EventsWebApplication.Domain.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EventsWebApplication.Domain.Abstractions.Data;

public interface IRepository<TEntity> where TEntity : Entity
{
    public Task<IReadOnlyList<TEntity>> ListAllAsync(CancellationToken cancellationToken = default);
    public Task<IReadOnlyList<TEntity>> PaginatedListAllAsync(int offset, int limit, CancellationToken cancellationToken = default);
    public Task<IReadOnlyList<TEntity>> ListAsync(Expression<Func<TEntity, bool>> filter, CancellationToken cancellationToken = default, params Expression<Func<TEntity, object>>[]? includesProperties);
    public Task<IReadOnlyList<TEntity>> PaginatedListAsync(Expression<Func<TEntity, bool>> filter, int offset, int limit, CancellationToken cancellationToken = default, params Expression<Func<TEntity, object>>[]? includesProperties);
    public Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    public Task Update(TEntity entity, CancellationToken cancellationToken = default);
    public Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);
    public Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default, params Expression<Func<TEntity, object>>[]? includesProperties);
    public Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> filter, CancellationToken cancellationToken = default);
    public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter, CancellationToken cancellationToken = default);
}
