using Application.Interfaces;
using Quartz;

namespace Presentation.BackgroundWorkers;

public class SendWeeklyEmailJob(INewsletterService newsletterService, ILogger<SendWeeklyEmailJob> logger)
    : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        logger.LogInformation("🚀 Quartz Job triggered: SendWeeklyEmailJob.");

        try
        {
            await newsletterService.SendWeeklyNewsletterAsync(context.CancellationToken);
            logger.LogInformation("✅ Weekly email job finished successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "❌ Failed to run weekly email job.");
            throw;
        }
    }
}