using DataAccess.Types;

namespace Application.Models.FeedSource;

public record FeedSourceCreateRequest(Category Category, string Url);