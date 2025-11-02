using Application.Interfaces;
using DataAccess;
using DataAccess.Entities;
using DataAccess.Interfaces;
using DataAccess.Types;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class FeedService(IFeedItemRepository feedItemRepo) : IFeedService
{
    public async Task<List<FeedItem>> GetAllFeedsAsync(CancellationToken cancellationToken)
    {
        return await feedItemRepo.GetAllAsync(cancellationToken);
    }

    public async Task<List<FeedItem>> GetFeedsByCategoryAsync(Category category, CancellationToken cancellationToken)
    {
        return await feedItemRepo.GetByCategoryAsync(category, cancellationToken);
    }

    public async Task<FeedItem?> GetFeedByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await feedItemRepo.GetByIdAsync(id, cancellationToken);
    }

    public async Task<List<FeedItem>> GetTopFeedsSinceAsync(DateTime cutoffDate, CancellationToken cancellationToken)
    {
        // ✅ الحل: استدعاء الدالة مباشرة من IFeedItemRepository (طبقة DataAccess)
        // هذا يفترض أنك قمت بتعديل الواجهة والتطبيق في طبقة DataAccess.

        return await feedItemRepo.GetTopFeedsSinceAsync(cutoffDate, cancellationToken);
    }
}
