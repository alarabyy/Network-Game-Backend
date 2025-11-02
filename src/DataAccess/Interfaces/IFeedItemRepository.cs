using DataAccess.Entities;
using DataAccess.Types;

namespace DataAccess.Interfaces;

public interface IFeedItemRepository
{
    Task<List<FeedItem>> GetAllAsync(CancellationToken cancellationToken);
    Task<List<FeedItem>> GetByCategoryAsync(Category category, CancellationToken cancellationToken);
    Task<FeedItem?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<bool> ExistsAsync(string link, CancellationToken cancellationToken);
    Task InsertAsync(FeedItem item, CancellationToken cancellationToken);
    Task<List<FeedItem>> GetTopFeedsSinceAsync(DateTime cutoffDate, CancellationToken cancellationToken);
}