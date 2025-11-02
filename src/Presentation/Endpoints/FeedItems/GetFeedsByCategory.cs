using Application.Interfaces;
using DataAccess.Entities;
using DataAccess.Types;
using FastEndpoints;

namespace Presentation.Endpoints.FeedItems;

public class GetFeedsByCategoryEndpoint(IFeedService feedService) : EndpointWithoutRequest<List<FeedItem>>
{
    public override void Configure()
    {
        Get("/api/feeds/category/{category}");
        Description(r => r.WithTags("Feeds"));
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var catParm = Route<int>("category");
        var category = Enum.Parse<Category>(catParm.ToString());
        var feeds = await feedService.GetFeedsByCategoryAsync(category, ct);
        await Send.OkAsync(feeds, ct);
    }
}
