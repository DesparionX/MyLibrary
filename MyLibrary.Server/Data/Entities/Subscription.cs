using MyLibrary.Server.Data.Entities.Interfaces;

namespace MyLibrary.Server.Data.Entities
{
    public class Subscription : ISubscription<string>
    {
        public required string Id { get; set; }
        public int? SubscriptionId { get; set; }
        public string? SubscriptionTier { get; set; }
        public string? UserId { get; set; }
        public int? Months { get; set; }
        public DateTime? ExpireDate { get; set; }
        
    }
}
