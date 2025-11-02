using Application.Interfaces;
using DataAccess.Interfaces;
using DataAccess.Entities;
using Application.Common;
using Application.DTOs;

namespace Application.Services;

public sealed class SubscriberService(ISubscriberRepository subscriberRepository) : ISubscriberService
{
    public async Task<List<SubscriberResponse>> GetAllSubscribersAsync(CancellationToken ct)
    {
        var subscribers = await subscriberRepository.GetAllAsync(ct);
        var dtos = subscribers.Select(s => new SubscriberResponse(s.Id, s.EmailAddress, s.IsActive, s.SubscriptionDate)).ToList();
        return dtos;
    }

    public async Task<StandardResponse<SubscriberResponse>> HandleSubscriptionAsync(SubscribeRequest request, CancellationToken ct)
    {
        if (string.IsNullOrEmpty(request.EmailAddress))
        {
            return StandardResponse<SubscriberResponse>.Failed("email is empty.");
        }

        var entity = new Subscriber
        {
            EmailAddress = request.EmailAddress,
            IsActive = true,
            SubscriptionDate = DateTime.UtcNow
        };
        
        await subscriberRepository.InsertAsync(entity, ct);
        var dto = new SubscriberResponse(entity.Id, entity.EmailAddress, entity.IsActive, entity.SubscriptionDate);
        return StandardResponse<SubscriberResponse>.Succeeded(dto, "Subscription successful.");
    }
}