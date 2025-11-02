using Application.Common;
using Application.Models.FeedSource;
using Application.Interfaces;
using FastEndpoints;

namespace Presentation.Endpoints.FeedSource;

public class UpdateFeedSourceEndpoint(IFeedSourceService service) : Endpoint<UpdateFeedSourceRequest, StandardResponse>
{
    public override void Configure()
    {
        Put("/api/feedsource/update/{id:int}");
        Description(s => s.WithTags("Feed Source"));
        AllowAnonymous();
    }

    public override async Task HandleAsync(UpdateFeedSourceRequest req, CancellationToken ct)
    {
        var id = Route<int>("id");
        var response = await service.UpdateFeedSourceAsync(id, req, ct);
        await Send.OkAsync(response, ct);
    }
}
