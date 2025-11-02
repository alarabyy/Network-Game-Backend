using DataAccess.Types;

namespace Application.Dtos;

public record FeedSourceDto(int Id, Category Category, string Url, DateTime? LastUpdated);