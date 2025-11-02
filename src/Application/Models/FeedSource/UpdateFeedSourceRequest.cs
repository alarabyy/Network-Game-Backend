using DataAccess.Types;

namespace Application.Models.FeedSource;

public record UpdateFeedSourceRequest(Category Category, string Url);