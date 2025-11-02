using Application.Common;
using Application.Dtos;
using Application.Interfaces;
using Application.Models.FeedSource;
using FastEndpoints;

namespace Presentation.Endpoints.FeedSource;

public class CreateFeedSourceEndpoint(IFeedSourceService service) : Endpoint<FeedSourceCreateRequest, StandardResponse>
{
    public override void Configure()
    {
        Post("/api/feedsource/create");
        Description(s => s.WithTags("Feed Source"));
        AllowAnonymous(); // TODO: when apply user auth make it not anonymous
    }

    public override async Task HandleAsync(FeedSourceCreateRequest req, CancellationToken ct)
    {
        var response = await service.CreateFeedSource(req, ct);
        await Send.OkAsync(response, ct);
    }
}