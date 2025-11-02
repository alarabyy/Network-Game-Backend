using Application.Interfaces;
using Application.DTOs;
using FastEndpoints;

namespace Presentation.Endpoints.Subscribers;

public class GetAllSubscribersEndpoint(ISubscriberService subscriberService) :
    EndpointWithoutRequest<List<SubscriberResponse>>
{
    public override void Configure()
    {
        Get("/api/subscribers");
        Description(r => r.WithTags("Subscribers"));
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var subscribers = await subscriberService.GetAllSubscribersAsync(ct);
        await Send.OkAsync(subscribers, ct);
    }
}