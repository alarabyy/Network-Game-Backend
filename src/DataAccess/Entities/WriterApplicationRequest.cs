using DataAccess.Types;

namespace DataAccess.Entities;

public class WriterApplicationRequest : BaseEntity
{
    public WriterType? Type { get; set; }
    public int UserId { get; set; }
    public DateTime ApplicationDate { get; set; } = DateTime.UtcNow;
    public DateTime? ApprovedDate { get; set; }
    
    public ApplicationUser? User { get; set; }
}