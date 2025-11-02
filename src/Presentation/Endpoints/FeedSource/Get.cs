using Application.Common;
using Application.Dtos;
using Application.Interfaces;
using FastEndpoints;

namespace Presentation.Endpoints.FeedSource;

public class GetFeedSourceEndpoint(IFeedSourceService service) : EndpointWithoutRequest<StandardResponse<List<FeedSourceDto>>>
{
    public override void Configure()
    {
        Get("/api/feedsource/get");
        Description(s => s.WithTags("Feed Source"));
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var response = await service.GetFeedSources(ct);
        await Send.OkAsync(response, ct);
    }
}