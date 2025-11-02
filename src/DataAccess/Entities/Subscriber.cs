namespace DataAccess.Entities;

public class Subscriber : BaseEntity
{
    public string EmailAddress { get; set; } = string.Empty;
    public bool IsActive { get; set; } // لمعرفة إذا كان المشترك لا يزال نشطاً
    public DateTime SubscriptionDate { get; set; }
    public DateTime? UnsubscribedDate { get; set; } // متى ألغى الاشتراك
}
