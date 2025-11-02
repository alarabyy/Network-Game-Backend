using Application.Interfaces;
using Application.Common;
using Application.Dtos;
using FastEndpoints;

namespace Presentation.Endpoints.FeedSource;

public class GetByIdFeedSourcesEndpoint(IFeedSourceService service) : EndpointWithoutRequest<StandardResponse<FeedSourceDto>>
{
    public override void Configure()
    {
        Get("/api/feedsource/get/{id:int}");
        Description(s => s.WithTags("Feed Source"));
        AllowAnonymous(); 
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = Route<int>("id");
        var response = await service.GetFeedSource(id, ct);
        await Send.OkAsync(response, ct);
    }
}