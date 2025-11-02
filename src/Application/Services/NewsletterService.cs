using Microsoft.Extensions.Logging;
using Application.Interfaces;
using DataAccess.Entities;
using DataAccess.Interfaces;

namespace Application.Services;

public class NewsletterService(
    ISubscriberRepository subscriberRepo,
    IFeedItemRepository feedItemRepo,
    IEmailSenderService emailService,
    ILogger<NewsletterService> logger) : INewsletterService
{
    public async Task SendWeeklyNewsletterAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting weekly newsletter dispatch.");
        var cutoffDate = DateTime.UtcNow.AddDays(-7);
        var weeklyNews = await feedItemRepo.GetTopFeedsSinceAsync(cutoffDate, cancellationToken);
        if (weeklyNews.Count == 0)
        {
            logger.LogWarning("No top news items found for this week. Skipping newsletter.");
            return;
        }
        
        var subscribers = await subscriberRepo.GetAllByAsync("Active", true, cancellationToken);
        logger.LogInformation("Found {Count} active subscribers.", subscribers.Count);

        if (subscribers.Count == 0) return;

        var htmlBody = CreateNewsletterHtml(weeklyNews);
        var sendTasks = subscribers.Select(subscriber =>
            emailService.SendEmailAsync(
                subscriber.EmailAddress,
                $"نشرة أخبار الأسبوع من موقعنا: {DateTime.Today:dd-MM-yyyy}",
                htmlBody,
                cancellationToken)
        ).ToList();
        
        await Task.WhenAll(sendTasks);
        logger.LogInformation("Weekly newsletter dispatch completed.");
    }

    private string CreateNewsletterHtml(List<FeedItem> news)
    {
        var html = "<h1>أهم أخبار الأسبوع!</h1>";
        html += "<ul>";
        foreach (var item in news.Take(5))
        {
            html += $"<li><a href='{item.Link}'>{item.Title}</a></li>";
        }
        html += "</ul><p>لإلغاء الاشتراك، اضغط هنا [Unsubscribe Link]</p>";
        return html;
    }
}