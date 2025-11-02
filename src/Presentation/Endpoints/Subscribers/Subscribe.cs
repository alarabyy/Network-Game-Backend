using Application.Interfaces;
using Application.Common;
using Application.DTOs;
using FastEndpoints;

namespace Presentation.Endpoints.Subscribers;

public class SubscribeEndpoint(ISubscriberService subscriberService) :
    Endpoint<SubscribeRequest, StandardResponse<SubscriberResponse>>
{
    public override void Configure()
    {
        Post("/api/subscribe");
        Description(r => r.WithTags("Subscribers"));
        AllowAnonymous();
    }

    public override async Task HandleAsync(SubscribeRequest req, CancellationToken ct)
    {
        var response = await subscriberService.HandleSubscriptionAsync(req, ct);
        await Send.OkAsync(response, ct);
    }
}