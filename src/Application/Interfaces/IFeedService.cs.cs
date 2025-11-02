using DataAccess.Entities;
using DataAccess.Types;

namespace Application.Interfaces;

public interface IFeedService
{
    Task<List<FeedItem>> GetAllFeedsAsync(CancellationToken cancellationToken);
    Task<List<FeedItem>> GetFeedsByCategoryAsync(Category category, CancellationToken cancellationToken);
    Task<FeedItem?> GetFeedByIdAsync(int id, CancellationToken cancellationToken); // خبر واحد بالـ Id
    Task<List<FeedItem>> GetTopFeedsSinceAsync(DateTime cutoffDate, CancellationToken cancellationToken);
}