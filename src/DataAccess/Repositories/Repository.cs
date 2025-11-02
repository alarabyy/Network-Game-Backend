using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using DataAccess.Interfaces;
using DataAccess.Entities;

namespace DataAccess.Repositories;

public abstract class Repository<T>(AppDbContext context) : IRepository<T> where T : BaseEntity
{
    protected readonly AppDbContext Context = context;

    public async Task<T?> GetAsync(int id, CancellationToken ct)
    {
        var entity = await Context.Set<T>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct) as T;
        return entity;
    } 
    public async Task<T?> GetByAsync(string columnName, object value, CancellationToken ct)
    {
        var query = BuildQuery(columnName, value);
        return await Context.Set<T>().AsNoTracking().FirstOrDefaultAsync(query, ct);
    }
    
    public async Task<List<T>> GetAllAsync(CancellationToken ct)
    {
        return await Context.Set<T>().AsNoTracking().ToListAsync(ct);
    }
    public async Task<List<T>> GetAllByAsync(string columnName, object value, CancellationToken ct)
    {
        var query = BuildQuery(columnName, value);
        return await Context.Set<T>().AsNoTracking().Where(query).ToListAsync<T>(ct);
    }
    
    public async Task InsertAsync(T entity, CancellationToken ct)
    {
        await Context.Set<T>().AddAsync(entity, ct);
        await Context.SaveChangesAsync(ct);
    }
    
    public async Task UpdateAsync(T entity, CancellationToken ct)
    {
        Context.Set<T>().Update(entity);
        await Context.SaveChangesAsync(ct);
    }
    
    public async Task DeleteAsync(T entity, CancellationToken ct)
    {
        Context.Remove(entity);
        await Context.SaveChangesAsync(ct);
    }
    public async Task DeleteAsync(IEnumerable<T> entities, CancellationToken ct)
    {
        Context.RemoveRange(entities);
        await Context.SaveChangesAsync(ct);
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken ct)
    {
        return await Context.Set<T>().AsNoTracking().AnyAsync(x => x.Id == id, ct);
    }

    // helpers
    private Expression<Func<T,bool>> BuildQuery(string columnName, object value)
    {
        // Validate column name exists on the entity
        var property = typeof(T).GetProperty(columnName);
        if (property == null)
        {
            throw new ArgumentException($"Column '{columnName}' does not exist on entity {typeof(T).Name}");
        } 
        // construct the query
        var parameter = Expression.Parameter(typeof(T), "x");
        var propertyAccess = Expression.Property(parameter, property);
        var constant = Expression.Constant(value);
        var equals = Expression.Equal(propertyAccess, constant);
        var lambda = Expression.Lambda<Func<T, bool>>(equals, parameter);

        return lambda;
    }

    public async Task<List<FeedItem>> GetTopFeedsSinceAsync(DateTime cutoffDate, CancellationToken cancellationToken)
    {
        return await Context.Set<FeedItem>()
            .AsNoTracking()
            // ⚠️ تأكد من أنك تستخدم Context هنا (وليس _context)
            .Where(f => f.PublishedAt.HasValue && f.PublishedAt.Value >= cutoffDate)
            .OrderByDescending(f => f.PublishedAt)
            .Take(10)
            .ToListAsync(cancellationToken);
    }
}