using DataAccess.Types;

namespace DataAccess.Entities;

public class FeedItem : BaseEntity
{
    public string Title { get; set; }
    public Category Category { get; set; }
    public string? Link { get; set; }
    public string? Author { get; set; }
    public string? Summary { get; set; }
    public string? Content { get; set; }
    public string? ImageUrl { get; set; }
    public string? Guid { get; set; }
    public string? SourceName { get; set; }
    public string? SourceUrl { get; set; }
    public string? Language { get; set; }
    public string? MediaContentUrl { get; set; }
    public DateTime? PublishedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime FetchedAt { get; set; } = DateTime.UtcNow;
    public bool Approved { get; set; } = true;
    
    public int? WriterId { get; set; }
    public int? SourceId { get; set; }  
    
    public ApplicationUser? Writer { get; set; }
    public FeedSource? Source { get; set; }
}