using System.Text.RegularExpressions;
using System.Xml.Linq;
using CodeHollow.FeedReader;
using DataAccess.Entities;
using DataAccess.Interfaces;
using Newtonsoft.Json;
using FeedItem = DataAccess.Entities.FeedItem;

namespace Presentation.BackgroundWorkers;

public class RssDiscoveryWorker(ILogger<RssDiscoveryWorker> logger, IServiceProvider services)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("🟢 RSS Background Worker Started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = services.CreateScope();
                var feedSourceRepo = scope.ServiceProvider.GetRequiredService<IFeedSourceRepository>();
                var feedItemRepo = scope.ServiceProvider.GetRequiredService<IFeedItemRepository>();
                var sources = await feedSourceRepo.GetAllAsync(stoppingToken);

                foreach (var source in sources)
                    await FetchFeedAsync(source, feedItemRepo, feedSourceRepo, logger, stoppingToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "💥 Unexpected error in RSS Worker");
            }

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }

    private static async Task FetchFeedAsync(
        FeedSource source,
        IFeedItemRepository feedItemRepo,
        IFeedSourceRepository feedSourceRepo,
        ILogger logger,
        CancellationToken stoppingToken)
    {
        try
        {
            var feedUrl = source.Url;
            if (string.IsNullOrWhiteSpace(feedUrl))
            {
                logger.LogWarning("⚠️ Missing URL for source {SourceId}", source.Id);
                return;
            }

            if (!feedUrl.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                feedUrl = "https://" + feedUrl;

            logger.LogInformation("📥 Fetching feed: {FeedUrl}", feedUrl);

            var feed = await FeedReader.ReadAsync(feedUrl, stoppingToken);

            if (feed?.Items == null || !feed.Items.Any())
            {
                logger.LogWarning("⚠️ No items found at {FeedUrl}", feedUrl);
                return;
            }

            foreach (var item in feed.Items)
            {
                var link = item.Link;
                if (string.IsNullOrEmpty(link))
                    continue;

                if (await feedItemRepo.ExistsAsync(link, stoppingToken))
                    continue;

                var imageUrl = ExtractImageUrl(item);
                if (string.IsNullOrEmpty(imageUrl))
                {
                    logger.LogDebug("⏭ Skipped '{Title}' (no image found)", item.Title ?? "(no title)");
                    continue; // skip items without an image
                }

                var newItem = new FeedItem
                {
                    Title = item.Title ?? "(no title)",
                    Link = item.Link,
                    Summary = item.Description,
                    Content = item.Content ?? item.Description,
                    Author = item.Author,
                    PublishedAt = item.PublishingDate ?? DateTime.UtcNow,
                    UpdatedAt = item.PublishingDate,
                    SourceName = feed.Title ?? "Unknown Source",
                    SourceUrl = feed.Link ?? feedUrl,
                    Category = source.Category,
                    Language = feed.Language,
                    Guid = item.Id,
                    FetchedAt = DateTime.UtcNow,
                    SourceId = source.Id,
                    ImageUrl = imageUrl
                };

                var xml = item.SpecificItem.Element;
                if (xml != null)
                {
                    var extra = xml.Elements()
                        .GroupBy(e => e.Name.LocalName)
                        .ToDictionary(
                            g => g.Key,
                            g => string.Join(" | ", g.Select(e => e.Value))
                        );

                    newItem.Content += "\n\n--- RAW DATA ---\n" + JsonConvert.SerializeObject(extra);
                }

                await feedItemRepo.InsertAsync(newItem, stoppingToken);
                logger.LogInformation("✅ Added item: {Title}", newItem.Title);
            }

            await feedSourceRepo.UpdateLastCheckedAsync(source.Id, stoppingToken);
            logger.LogInformation("✔ Completed fetching {FeedUrl}", feedUrl);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "❌ Error while fetching feed {FeedUrl}", source.Url);
        }
    }

    // 🧩 Helper: Extract image URL from multiple possible RSS/Atom formats
    private static string? ExtractImageUrl(CodeHollow.FeedReader.FeedItem item)
    {
        try
        {
            // 1️⃣ Check for standard enclosure
            // if (!string.IsNullOrWhiteSpace(item.Enclosure?.Url))
            //     return item.Enclosure.Url;

            // 2️⃣ Look into XML structure (RDF / RSS / Atom)
            var xml = item.SpecificItem.Element;
            if (xml != null)
            {
                // Try <enclosure url="..." />
                var enclosure = xml.Descendants().FirstOrDefault(e =>
                    e.Name.LocalName.Equals("enclosure", StringComparison.OrdinalIgnoreCase));
                var encUrl = enclosure?.Attribute("url")?.Value;
                if (!string.IsNullOrWhiteSpace(encUrl))
                    return encUrl;

                // Try <media:content> or <media:thumbnail>
                var mrssNs = "http://search.yahoo.com/mrss/";
                var media = xml.Descendants(XName.Get("content", mrssNs)).FirstOrDefault()
                         ?? xml.Descendants(XName.Get("thumbnail", mrssNs)).FirstOrDefault();
                var mediaUrl = media?.Attribute("url")?.Value ?? media?.Attribute("href")?.Value;
                if (!string.IsNullOrWhiteSpace(mediaUrl))
                    return mediaUrl;

                // Try <image><url>...</url></image>
                var image = xml.Descendants().FirstOrDefault(e => e.Name.LocalName == "image");
                var imageUrl = image?.Value
                             ?? image?.Attribute("href")?.Value
                             ?? image?.Attribute("url")?.Value;
                if (!string.IsNullOrWhiteSpace(imageUrl))
                    return imageUrl;
            }

            // 3️⃣ Fallback: parse <img src="..."> from description/content HTML
            var html = item.Content ?? item.Description;
            if (!string.IsNullOrEmpty(html))
            {
                var m = Regex.Match(html, "<img[^>]+src\\s*=\\s*[\"'](?<src>[^\"']+)[\"']", RegexOptions.IgnoreCase);
                if (m.Success)
                    return m.Groups["src"].Value;
            }
        }
        catch
        {
            // ignore parsing errors
        }

        return null;
    }
}
