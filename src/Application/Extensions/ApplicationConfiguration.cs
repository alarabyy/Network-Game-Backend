using Microsoft.Extensions.DependencyInjection;
using Application.Interfaces;
using Application.Services;

namespace Application.Extensions;

public static class ApplicationConfiguration
{
    public static IServiceCollection AddApplicationConfiguration(this IServiceCollection services)
    {
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IEmailSenderService, EmailSenderService>();
        services.AddScoped<IFeedSourceService, FeedSourceService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IFeedService, FeedService>();
        services.AddScoped<INewsletterService , NewsletterService>();
        services.AddScoped<ISubscriberService, SubscriberService>();
        services.AddScoped<IWriterService, WriterService>();

        return services;
    }
}