using MyLibrary.Server.Data.Entities.Interfaces;

namespace MyLibrary.Server.Data.Entities
{
    public class SubscriptionTier : ISubscriptionTier
    {
        public int Id { get; set; }
        public string? Tier { get; set; }
        public decimal? MonthlyCost { get; set; }
        
    }
}
