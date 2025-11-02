using DataAccess.Entities;
using DataAccess.Types;

namespace DataAccess.Interfaces;

public interface IFeedSourceRepository : IRepository<FeedSource>
{
    Task<bool> ExistsAsync(Category category, string url, CancellationToken ct);
    Task UpdateLastCheckedAsync(int sourceId, CancellationToken ct);
}
