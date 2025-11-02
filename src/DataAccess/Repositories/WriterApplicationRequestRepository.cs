using Microsoft.EntityFrameworkCore;
using DataAccess.Interfaces;
using DataAccess.Entities;

namespace DataAccess.Repositories;

public class WriterApplicationRequestRepository(AppDbContext context) 
    : Repository<WriterApplicationRequest>(context), IWriterApplicationRequestRepository
{
    public Task<List<WriterApplicationRequest>> GetAllWithUserAsync(CancellationToken ct)
    {
        return Context.WriterApplicationRequests
            .Include(r => r.User)
            .OrderByDescending(r => r.ApplicationDate)
            .ToListAsync(ct);
    }
    public Task<WriterApplicationRequest?> GetWithUserAsync(int id, CancellationToken ct)
    {
        return Context.WriterApplicationRequests
            .Include(r => r.User)
            .FirstOrDefaultAsync(r => r.Id == id, ct);
    }

    public Task<bool> UserHaveUnprocessedRequestAsync(int userId, CancellationToken ct)
    {
        return Context.WriterApplicationRequests
            .AnyAsync(r => r.UserId == userId && r.Type == null, ct);
    }
}