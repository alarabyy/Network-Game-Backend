namespace Application.DTOs;

public record SubscriberResponse(
    int Id,
    string EmailAddress,
    bool IsActive,
    DateTime SubscriptionDate
);

public record SubscribeRequest(
    string EmailAddress
);