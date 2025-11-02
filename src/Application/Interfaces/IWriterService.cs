using Application.Common;
using Application.Dtos;
using Application.Models.Writer;
using DataAccess.Types;

namespace Application.Interfaces;

public interface IWriterService
{
    Task<List<WriterApplicationRequestDto>> GetAllAsync(CancellationToken ct);
    Task<StandardResponse> ApplyAsync(int id, CancellationToken ct);
    Task<StandardResponse> AcceptAsync(WriterApplicationAcceptanceRequest request, CancellationToken ct);
    Task<StandardResponse> RemoveAsync(int id, CancellationToken ct);
}