using Application.Common;
using Application.DTOs;

namespace Application.Interfaces;

public interface ISubscriberService
{
    Task<List<SubscriberResponse>> GetAllSubscribersAsync(CancellationToken ct);
    Task<StandardResponse<SubscriberResponse>> HandleSubscriptionAsync(SubscribeRequest request, CancellationToken ct);
}