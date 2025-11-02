using DataAccess.Interfaces;
using DataAccess.Entities;
using DataAccess.Types;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories;

public class FeedSourceRepository(AppDbContext context) : Repository<FeedSource>(context), IFeedSourceRepository
{
    public async Task<bool> ExistsAsync(Category category, string url, CancellationToken ct)
    {
        return await Context.FeedSources.AnyAsync(s => s.Category == category && s.Url == url, ct);
    }
    
    public async Task UpdateLastCheckedAsync(int sourceId, CancellationToken ct)
    {
        var source = await Context.FeedSources.FindAsync([sourceId], ct);
        if (source is null) return;

        source.LastChecked = DateTime.UtcNow;
        await Context.SaveChangesAsync(ct);
    }
}