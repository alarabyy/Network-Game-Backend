namespace DataAccess.Interfaces;

public interface IRepository<T>
{
    Task<T?> GetAsync(int id, CancellationToken cancellationToken);
    Task<T?> GetByAsync(string columnName, object value, CancellationToken ct);
    Task<List<T>> GetAllAsync(CancellationToken cancellationToken);
    Task<List<T>> GetAllByAsync(string columnName, object value, CancellationToken ct);
    Task InsertAsync(T entity, CancellationToken ct);
    Task UpdateAsync(T entity, CancellationToken ct);
    Task DeleteAsync(T entity, CancellationToken ct);
    Task DeleteAsync(IEnumerable<T> entities, CancellationToken ct);
    
    Task<bool> ExistsAsync(int id, CancellationToken ct);
}