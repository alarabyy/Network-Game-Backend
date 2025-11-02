using DataAccess.Entities;
using DataAccess.Interfaces;
using DataAccess.Types;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class FeedItemRepository(AppDbContext context) : IFeedItemRepository
    {
        public async Task<List<FeedItem>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await context.FeedItems.ToListAsync(cancellationToken);
        }

        public async Task<List<FeedItem>> GetByCategoryAsync(Category category, CancellationToken cancellationToken)
        {
            return await context.FeedItems
                .Where(f => f.Category == category)
                .ToListAsync(cancellationToken);
        }

        public async Task<FeedItem?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await context.FeedItems
                .FirstOrDefaultAsync(f => f.Id == id, cancellationToken);
        }

        public async Task<bool> ExistsAsync(string link, CancellationToken cancellationToken)
        {
            return await context.FeedItems.AnyAsync(f => f.Link == link, cancellationToken);
        }

        public async Task InsertAsync(FeedItem item, CancellationToken cancellationToken)
        {
            await context.FeedItems.AddAsync(item, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }
        // 🚀 تنفيذ الدالة الخاصة هنا (لأنها الآن في الكلاس الصحيح)
        public async Task<List<FeedItem>> GetTopFeedsSinceAsync(DateTime cutoffDate, CancellationToken cancellationToken)
        {
            // 1. استخدم 'Context' الموروثة (C كبيرة)
            // 2. استخدم .Set<FeedItem>() لجلب البيانات
            return await context.Set<FeedItem>()
                .AsNoTracking()
                .Where(f => f.PublishedAt.HasValue && f.PublishedAt.Value >= cutoffDate)
                .OrderByDescending(f => f.PublishedAt)
                .Take(10)
                .ToListAsync(cancellationToken);
        }
    }
}
