using Application.Models.FeedSource;
using Application.Interfaces;
using DataAccess.Interfaces;
using DataAccess.Entities;
using Application.Common;
using Application.Dtos;
using DataAccess.Types;

namespace Application.Services;

public class FeedSourceService(IFeedSourceRepository feedSourceRepository) : IFeedSourceService
{
    public async Task<StandardResponse<FeedSourceDto>> GetFeedSource(int id, CancellationToken ct)
    {
        var source = await feedSourceRepository.GetAsync(id, ct);

        if (source is null)
            return StandardResponse<FeedSourceDto>.Failed("Feed source not found");

        var dto = new FeedSourceDto(
            source.Id,
            source.Category,
            source.Url ?? string.Empty,
            source.LastChecked ?? DateTime.MinValue
        );

        return StandardResponse<FeedSourceDto>.Succeeded(dto);
    }

     //🟢 Get All Feed Sources
    public async Task<StandardResponse<List<FeedSourceDto>>> GetFeedSources(CancellationToken ct)
    {
        var entities = await feedSourceRepository.GetAllAsync(ct);

        if (entities == null || !entities.Any())
            return StandardResponse<List<FeedSourceDto>>.Succeeded(new List<FeedSourceDto>());

        var dtos = entities.Select(entity =>
            new FeedSourceDto(
                entity.Id,
                entity.Category,
                entity.Url ?? string.Empty,
                entity.LastChecked ?? DateTime.MinValue
            )
        ).ToList();

        return StandardResponse<List<FeedSourceDto>>.Succeeded(dtos);
    }
    
    public async Task<StandardResponse> CreateFeedSource(FeedSourceCreateRequest request, CancellationToken ct)
    {
        var exists = await feedSourceRepository.ExistsAsync(request.Category, request.Url, ct);
        if (exists)
        {
            return StandardResponse.Failed("Source URL already exists in this category.");
        }

        var entity = new FeedSource
        {
            Category = request.Category,
            Url = request.Url
        };

        await feedSourceRepository.InsertAsync(entity, ct);
        
        return StandardResponse.Succeeded("Added successfully");
    }
    
    // public async Task<StandardResponse> DeleteFeedSource(int id, CancellationToken ct)
    // {
    //     var existing = await feedSourceRepository.GetAsync(id, ct);
    //     if (existing is null)
    //         return StandardResponse.Failed("Feed source not found.");
    //
    //     await feedSourceRepository.DeleteAsync(existing, ct);
    //
    //     return StandardResponse.Succeeded("Deleted successfully");
    // }

    public async Task<StandardResponse> UpdateFeedSourceAsync(int id, UpdateFeedSourceRequest request, CancellationToken ct)
    {
        var feedSource = await feedSourceRepository.GetAsync(id, ct);

        if (feedSource == null)
            return StandardResponse.Failed("Feed source not found.");

        
        feedSource.Category = request.Category;
        feedSource.Url = request.Url.Trim();
        feedSource.LastChecked = DateTime.UtcNow;

        await feedSourceRepository.UpdateAsync(feedSource, ct);

        return StandardResponse.Succeeded("Feed source updated successfully.");
    }

}