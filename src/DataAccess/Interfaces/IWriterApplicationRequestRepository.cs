using DataAccess.Entities;

namespace DataAccess.Interfaces;

public interface IWriterApplicationRequestRepository : IRepository<WriterApplicationRequest>
{
    Task<List<WriterApplicationRequest>> GetAllWithUserAsync(CancellationToken ct);
    Task<WriterApplicationRequest?> GetWithUserAsync(int id, CancellationToken ct);
    Task<bool> UserHaveUnprocessedRequestAsync(int userId, CancellationToken ct);
}