using DataAccess.Types;

namespace DataAccess.Entities;

public class FeedSource : BaseEntity
{
    public string? Url { get; set; }
    public Category Category { get; set; }
    public DateTime? LastChecked { get; set; }
}