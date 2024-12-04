using EventsWebApplication.Domain.Primitives;
using EventsWebApplication.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EventsWebApplication.Infrastructure.Repository;

internal class AppRepository<TEntity> : IRepository<TEntity> where TEntity : Entity
{
    protected readonly ApplicationDbContext _context;
    protected readonly DbSet<TEntity> _entities;

    public AppRepository(ApplicationDbContext context)
    {
        _context = context;
        _entities = context.Set<TEntity>();
    }

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await _entities.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        _entities.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> filter, CancellationToken cancellationToken = default)
    {
        return await _entities.FirstOrDefaultAsync(filter, cancellationToken);
    }

    public async Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default, params Expression<Func<TEntity, object>>[]? includesProperties)
    {
        IQueryable<TEntity> query = _entities;

        if (includesProperties != null)
        {
            foreach (var includeProperty in includesProperties)
            {
                query = query.Include(includeProperty);
            }
        }

        return await query.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<TEntity>> ListAllAsync(CancellationToken cancellationToken = default)
    {
        return await _entities.ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<TEntity>> PaginatedListAllAsync(int offset, int limit, CancellationToken cancellationToken = default)
    {
        return await _entities
            .Skip(offset)
            .Take(limit)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<TEntity>> ListAsync(Expression<Func<TEntity, bool>>? filter, CancellationToken cancellationToken = default, params Expression<Func<TEntity, object>>[]? includesProperties)
    {
        IQueryable<TEntity> query = _entities.AsQueryable();

        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (includesProperties != null)
        {
            foreach (var includeProperty in includesProperties)
            {
                query = query.Include(includeProperty);
            }
        }

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<TEntity>> PaginatedListAsync(Expression<Func<TEntity, bool>>? filter, int offset, int limit, CancellationToken cancellationToken = default, params Expression<Func<TEntity, object>>[]? includesProperties)
    {
        IQueryable<TEntity> query = _entities.AsQueryable();

        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (includesProperties != null)
        {
            foreach (var includeProperty in includesProperties)
            {
                query = query.Include(includeProperty);
            }
        }

        return await query
            .Skip(offset)
            .Take(limit)
            .ToListAsync(cancellationToken);
    }

    public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        _entities.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
