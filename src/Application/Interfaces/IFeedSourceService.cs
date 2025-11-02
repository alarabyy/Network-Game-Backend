using Application.Common;
using Application.Dtos;
using Application.Models.FeedSource;
using DataAccess.Types;

namespace Application.Interfaces;

public interface IFeedSourceService
{
    Task<StandardResponse<FeedSourceDto>> GetFeedSource(int id, CancellationToken ct);
    Task<StandardResponse<List<FeedSourceDto>>> GetFeedSources(CancellationToken ct);
    Task<StandardResponse> CreateFeedSource(FeedSourceCreateRequest request, CancellationToken ct);
    //Task<StandardResponse> DeleteFeedSource(int id, CancellationToken ct);
    Task<StandardResponse> UpdateFeedSourceAsync(int id, UpdateFeedSourceRequest request, CancellationToken ct);
}